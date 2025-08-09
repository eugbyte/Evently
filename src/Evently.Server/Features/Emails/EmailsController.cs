using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Domains.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;

namespace Evently.Server.Features.Emails;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class EmailsController(
	ChannelWriter<EmailMqPayload> writer,
	IBookingService bookingService) : ControllerBase {
	[HttpPost("{email}/preview/attendee-tickets/{bookingEventId}", Name = "EmailAttendeeTicket")]
	public async Task<ActionResult> EmailAttendeeTicket(string email, string bookingEventId) {
		string html = await bookingService.RenderMemberTicket(bookingEventId);
		await writer.WriteAsync(new EmailMqPayload(email, html));
		return Ok("Email sent successfully");
	}

	[HttpGet("preview/attendee-tickets/{bookingEventId}", Name = "PreviewEmail")]
	public async Task<ActionResult> PreviewEmail(string bookingEventId) {
		string bodyEmail = await bookingService.RenderMemberTicket(bookingEventId);
		return Content(bodyEmail, "text/html");
	}
}