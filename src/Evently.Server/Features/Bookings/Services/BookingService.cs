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
	AppDbContext db,
	IFileStorageService fileStorageService,
	IMediaRenderer mediaRenderer)
	: IBookingService {
	public async Task<Booking?> GetBooking(string bookingId) {
		return await db.Bookings
			.Include((b) => b.Member)
			.Include((b) => b.Gathering)
			.FirstOrDefaultAsync((be) => be.BookingId == bookingId);
	}

	public async Task<PageResult<Booking>> GetBookings(long? guestMemberId, long? hostMemberId,
		DateTime? checkInStart, DateTime? checkInEnd,
		bool? isCancelled, int? offset, int? limit) {
		IQueryable<Booking> query = db.Bookings
			.AsSplitQuery()
			.Where((b) => guestMemberId == null || b.MemberId == guestMemberId)
			.Where((b) => hostMemberId == null || b.GatheringId == hostMemberId)
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

	public async Task<Booking> CreateBooking(BookingDto bookingDto) {
		Booking booking = bookingDto.ToBooking();
		booking.BookingId = $"book_{await Nanoid.GenerateAsync(size: 10)}";
		await db.Bookings.AddAsync(booking);
		await db.SaveChangesAsync();
		return booking;
	}

	public async Task<Booking> UpdateBooking(string bookingId, BookingDto bookingDto) {
		Booking booking = bookingDto.ToBooking();

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

	public async Task<string> RenderMemberTicket(string bookingId) {
		Booking? bookingEvent = await db.Bookings.FindAsync(bookingId);
		if (bookingEvent is null) {
			throw new KeyNotFoundException($"BookingEvent with id: {bookingId} not found");
		}

		Member? attendee = await db.Members.FindAsync(bookingEvent.MemberId);
		Gathering? gathering = await db.Gatherings.FindAsync(bookingEvent.GatheringId);
		if (gathering is null || attendee is null) {
			throw new KeyNotFoundException($"Attendee or Exhibition with id: {bookingId} not found");
		}

		string qrData = JsonSerializer.Serialize(new { bookingEventId = bookingId });
		BinaryData binaryData = mediaRenderer.RenderQr(qrData);
		string fileName = $"booking-events/{bookingId}/qrcode.png";

		Uri uri;
		bool isFileExists = await fileStorageService.IsFileExists(fileName);
		if (!isFileExists) {
			uri = await fileStorageService.UploadFile(fileName, binaryData, "image/png");
		} else {
			uri = await fileStorageService.GetFileUri(fileName);
		}

		Dictionary<string, object?> props = new() {
			{ "BookingEvent", bookingEvent },
			{ "Attendee", attendee },
			{ "Gathering", gathering },
			{ "QrCodeUrl", uri.AbsoluteUri },
		};

		return await mediaRenderer.RenderComponentHtml<AttendeeTicket>(props);
	}
}