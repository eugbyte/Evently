using Evently.Server.Common.Adapters.Data;
using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Domains.Models;
using Evently.Server.Common.Extensions;
using Evently.Server.Features.Emails.Views;
using Microsoft.EntityFrameworkCore;
using NanoidDotNet;
using System.Text.Json;

namespace Evently.Server.Features.Bookings.Services;

public sealed class BookingService(
	IMediaRenderer mediaRenderer,
	IFileStorageService fileStorageService,
	AppDbContext db)
	: IBookingService {
	public async Task<Booking?> GetBooking(string bookingId) {
		return await db.Bookings
			.Include((b) => b.Member)
			.Include((b) => b.Gathering)
			.FirstOrDefaultAsync((be) => be.BookingId == bookingId);
	}

	public async Task<PageResult<Booking>> GetBookings(long? guestId, long? organiserId,
		DateTime? checkInStart, DateTime? checkInEnd,
		bool? isCancelled, int? offset, int? limit) {
		IQueryable<Booking> query = db.Bookings
			.Where((b) => guestId == null || b.MemberId == guestId)
			.Where((b) => organiserId == null || b.GatheringId == organiserId)
			.Where((c) => checkInStart == null || checkInStart <= c.CheckInDateTime)
			.Where((b) => checkInEnd == null || b.CheckInDateTime <= checkInEnd)
			.Where((b) => isCancelled == null || b.CancellationDateTime.HasValue == isCancelled)
			.Include((b) => b.Member)
			.Include((b) => b.Gathering);

		int totalCount = await query.CountAsync();

		List<Booking> bookingEvents = await query
			.OrderByDescending((be) => be.RegistrationDateTime)
			.Skip(offset ?? 0)
			.Take(limit ?? int.MaxValue)
			.ToListAsync();

		return new PageResult<Booking> {
			Items = bookingEvents,
			TotalCount = totalCount,
		};
	}

	public async Task<bool> Exists(string bookingId) {
		return await db.Bookings.AnyAsync(b => b.BookingId == bookingId);
	}

	public async Task<Booking> CreateBooking(BookingReqDto bookingReqDto) {
		Booking booking = bookingReqDto.ToBooking();
		booking.BookingId = $"book_{await Nanoid.GenerateAsync(size: 10)}";
		await db.Bookings.AddAsync(booking);
		await db.SaveChangesAsync();
		return booking;
	}

	public async Task<Booking> UpdateBooking(string bookingId, BookingReqDto bookingReqDto) {
		Booking booking = bookingReqDto.ToBooking();

		Booking current = await db.Bookings.AsTracking()
			                  .FirstOrDefaultAsync((be) => be.BookingId == bookingId)
		                  ?? throw new KeyNotFoundException($"{booking.BookingId} not found");

		current.MemberId = booking.MemberId;
		current.GatheringId = booking.GatheringId;
		current.RegistrationDateTime = booking.RegistrationDateTime;
		current.CheckInDateTime = booking.CheckInDateTime;
		current.CheckoutDateTime = booking.CheckoutDateTime;
		current.CancellationDateTime = booking.CancellationDateTime;

		await db.SaveChangesAsync();
		return current;
	}

	public async Task<string> RenderTicket(string bookingId) {
		Booking? booking = await GetBooking(bookingId);
		if (booking?.Member is null || booking.Gathering is null) {
			throw new KeyNotFoundException(
				$"Booking with id: {bookingId} not found or related member or gathering is null");
		}

		string qrData = JsonSerializer.Serialize(new { bookingEventId = bookingId });
		BinaryData binaryData = mediaRenderer.RenderQr(qrData);
		string fileName = $"bookings/{bookingId}/qrcode.png";

		Uri uri;
		bool isFileExists = await fileStorageService.IsFileExists(fileName);
		if (!isFileExists) {
			uri = await fileStorageService.UploadFile(fileName, binaryData, "image/png");
		} else {
			uri = await fileStorageService.GetFileUri(fileName);
		}

		Dictionary<string, object?> props = new() {
			{ "Booking", booking },
			{ "QrCodeUrl", uri.AbsoluteUri },
		};

		return await mediaRenderer.RenderComponentHtml<Ticket>(props);
	}
}