using Evently.Server.Common.Adapters.Data;
using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Domains.Models;
using Evently.Server.Common.Extensions;
using Evently.Server.Features.Emails.Views;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NanoidDotNet;
using System.Text.Json;
using ValidationResult=FluentValidation.Results.ValidationResult;

namespace Evently.Server.Features.Bookings.Services;

public sealed class BookingService(
	IMediaRenderer mediaRenderer,
	IFileStorageService fileStorageService,
	IValidator<Booking> validator,
	AppDbContext db)
	: IBookingService {
	public async Task<Booking?> GetBooking(string bookingId) {
		return await db.Bookings
			.Include((b) => b.Account)
			.Include((b) => b.Gathering)
			.ThenInclude((g) => g!.GatheringCategoryDetails)
			.ThenInclude((detail) => detail.Category)
			.FirstOrDefaultAsync((be) => be.BookingId == bookingId);
	}

	public async Task<PageResult<Booking>> GetBookings(string? accountId, long? gatheringId,
		DateTimeOffset? checkInStart, DateTimeOffset? checkInEnd,
		DateTimeOffset? gatheringStartBefore, DateTimeOffset? gatheringStartAfter, DateTimeOffset? gatheringEndBefore, DateTimeOffset? gatheringEndAfter,
		bool? isCancelled, int? offset, int? limit) {
		IQueryable<Booking> query = db.Bookings
			.Where((b) => accountId == null || b.AccountId == accountId)
			.Where((b) => gatheringId == null || b.GatheringId == gatheringId)
			.Where((c) => checkInStart == null || checkInStart <= c.CheckInDateTime)
			.Where((b) => checkInEnd == null || b.CheckInDateTime <= checkInEnd)
			.Where((b) => isCancelled == null || b.CancellationDateTime.HasValue == isCancelled)
			.Where((b) => gatheringStartBefore == null || b.Gathering != null && b.Gathering.Start <= gatheringStartBefore)
			.Where((b) => gatheringStartAfter == null || b.Gathering != null && b.Gathering.Start >= gatheringStartAfter)
			.Where((b) => gatheringEndBefore == null || b.Gathering != null && b.Gathering.End <= gatheringEndBefore)
			.Where((b) => gatheringEndAfter == null || b.Gathering != null && b.Gathering.End >= gatheringEndAfter)
			.Include((b) => b.Account)
			.Include((b) => b.Gathering)
			.ThenInclude((g) => g!.GatheringCategoryDetails)
			.ThenInclude((detail) => detail.Category);

		int totalCount = await query.CountAsync();

		List<Booking> bookingEvents = await query
			.OrderByDescending((be) => be.CreationDateTime)
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
		ValidationResult validationResult = await validator.ValidateAsync(booking);
		if (!validationResult.IsValid) {
			throw new ArgumentException(string.Join("\n", values: validationResult.Errors.Select(e => e.ErrorMessage)));
		}

		booking.BookingId = $"book_{await Nanoid.GenerateAsync(size: 10)}";
		await db.Bookings.AddAsync(booking);
		await db.SaveChangesAsync();
		return (await GetBooking(booking.BookingId))!;
	}

	public async Task<Booking> UpdateBooking(string bookingId, BookingReqDto bookingReqDto) {
		Booking booking = bookingReqDto.ToBooking();

		ValidationResult validationResult = await validator.ValidateAsync(booking);
		if (!validationResult.IsValid) {
			throw new ArgumentException(string.Join("\n", values: validationResult.Errors.Select(e => e.ErrorMessage)));
		}

		Booking current = await db.Bookings.AsTracking()
			                  .FirstOrDefaultAsync((be) => be.BookingId == bookingId)
		                  ?? throw new KeyNotFoundException($"{booking.BookingId} not found");

		current.AccountId = booking.AccountId;
		current.GatheringId = booking.GatheringId;
		current.CreationDateTime = booking.CreationDateTime;
		current.CheckInDateTime = booking.CheckInDateTime;
		current.CheckoutDateTime = booking.CheckoutDateTime;
		current.CancellationDateTime = booking.CancellationDateTime;

		await db.SaveChangesAsync();
		return (await GetBooking(booking.BookingId))!;
	}

	public async Task<string> RenderTicket(string bookingId) {
		Booking? booking = await GetBooking(bookingId);
		if (booking?.Account is null || booking.Gathering is null) {
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