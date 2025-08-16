using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Domains.Models;
using Evently.Server.Common.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Channels;

namespace Evently.Server.Features.Bookings;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class BookingsController(IBookingService bookingService, ChannelWriter<string> emailQueue, IValidator<Booking> validator)
	: ControllerBase {
	[HttpGet("{bookingId}", Name = "GetBooking")]
	public async Task<ActionResult<Booking>> GetBooking(string bookingId) {
		Booking? booking = await bookingService.GetBooking(bookingId);
		if (booking is null) {
			return NotFound();
		}

		return Ok(booking);
	}

	[HttpGet("{bookingId}/preview", Name = "PreviewBooking")]
	public async Task<ActionResult<Booking>> PreviewBooking(string bookingId) {
		string html = await bookingService.RenderTicket(bookingId);
		return Content(html);
	}

	[HttpGet("", Name = "GetBookings")]
	public async Task<ActionResult<Booking>> GetBookings(
		string? attendeeId,
		long? gatheringId,
		DateTimeOffset? checkInStart,
		DateTimeOffset? checkInEnd,
		DateTimeOffset? gatheringStartBefore, DateTimeOffset? gatheringStartAfter, DateTimeOffset? gatheringEndBefore, DateTimeOffset? gatheringEndAfter,
		bool isCancelled,
		int? offset,
		int? limit) {
		PageResult<Booking> result = await bookingService.GetBookings(attendeeId,
			gatheringId,
			checkInStart,
			checkInEnd,
			gatheringStartBefore,
			gatheringStartAfter,
			gatheringEndBefore,
			gatheringEndAfter,
			isCancelled,
			offset,
			limit);
		List<Booking> bookingEvents = result.Items;
		int total = result.TotalCount;
		HttpContext.Response.Headers.Append("Access-Control-Expose-Headers", "X-Total-Count");
		HttpContext.Response.Headers.Append("X-Total-Count", value: total.ToString(CultureInfo.InvariantCulture));
		return Ok(bookingEvents);
	}

	[HttpPost("", Name = "CreateBooking")]
	public async Task<ActionResult<Booking>> CreateBooking([FromBody] BookingReqDto bookingReqDto) {
		Booking booking = bookingReqDto.ToBooking();
		ValidationResult validationResult = await validator.ValidateAsync(booking);
		if (!validationResult.IsValid) {
			return BadRequest(validationResult.Errors);
		}

		booking = await bookingService.CreateBooking(bookingReqDto);
		await emailQueue.WriteAsync(booking.BookingId);
		return Ok(booking);
	}

	[HttpPut("{bookingId}", Name = "UpdateBooking")]
	public async Task<ActionResult> UpdateBooking(string bookingId,
		[FromBody] BookingReqDto bookingReqDto) {
		ValidationResult validationResult = await validator.ValidateAsync(bookingReqDto.ToBooking());
		if (!validationResult.IsValid) {
			return BadRequest(validationResult.Errors);
		}

		bool isExist = await bookingService.Exists(bookingId);
		if (!isExist) {
			return NotFound();
		}

		Booking booking = await bookingService.UpdateBooking(bookingId, bookingReqDto);
		return Ok(booking);
	}
}