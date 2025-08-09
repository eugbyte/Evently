using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Interfaces;
using Evently.Server.Domains.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Channels;

namespace Evently.Server.Features.Bookings;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class BookingsController(IBookingService bookingService, ChannelWriter<EmailMqPayload> writer)
	: ControllerBase {
	[HttpGet("{bookingId}", Name = "GetBooking")]
	public async Task<ActionResult<Booking>> GetBooking(string bookingId) {
		Booking? bookingEvent = await bookingService.GetBooking(bookingId);
		if (bookingEvent is null) {
			return NotFound();
		}

		return Ok(bookingEvent);
	}

	[HttpGet("{bookingId}/preview-emailed-ticket", Name = "PreviewEmailedTicket")]
	public async Task<ActionResult> PreviewEmailedTicket(string bookingId) {
		string html = await bookingService.RenderMemberTicket(bookingId);
		return Content(html, "text/html");
	}

	[HttpGet("", Name = "GetBookings")]
	public async Task<ActionResult<Booking>> GetBookings(
		long? attendeeId,
		long? exhibitionId,
		DateTime? checkInStart,
		DateTime? checkInEnd,
		bool isCancelled,
		int? offset,
		int? limit) {
		PageResult<Booking> result = await bookingService.GetBookings(attendeeId,
			exhibitionId,
			checkInStart,
			checkInEnd,
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
	public async Task<ActionResult<Booking>> CreateBooking([FromBody] BookingDto bookingDto) {
		Booking booking = await bookingService.CreateBooking(bookingDto);
		string bookingEventId = booking.BookingId;
		Booking?
			eagerBooking = await bookingService.GetBooking(bookingEventId); // eagerly load the attendee property
		if (eagerBooking?.Member is null) {
			return NotFound($"Attendee with bookingId {bookingEventId} not found");
		}

		Member member = eagerBooking.Member;
		string html = await bookingService.RenderMemberTicket(bookingEventId);
		await writer.WriteAsync(new EmailMqPayload(member.Email, html));
		return Ok(booking);
	}

	[HttpPut("{bookingId}", Name = "UpdateBooking")]
	public async Task<ActionResult> UpdateBooking(string bookingId,
		[FromBody] BookingDto bookingDto) {
		bool isExist = await bookingService.Exists(bookingId);
		if (!isExist) {
			return NotFound();
		}

		Booking booking = await bookingService.UpdateBooking(bookingId, bookingDto);
		return Ok(booking);
	}
}