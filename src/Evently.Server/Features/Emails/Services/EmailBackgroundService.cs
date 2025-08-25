using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Interfaces;
using System.Threading.Channels;
using LoggerExtension=Evently.Server.Common.Extensions.LoggerExtension;

namespace Evently.Server.Features.Emails.Services;

public sealed class EmailBackgroundService(
	IServiceScopeFactory scopeFactory,
	ChannelReader<string> reader,
	IEmailerAdapter emailerAdapter,
	ILogger<EmailBackgroundService> logger) : BackgroundService {
	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		while (await reader.WaitToReadAsync(stoppingToken)) {
			try {
				string bookingId = await reader.ReadAsync(stoppingToken);

				using IServiceScope scope = scopeFactory.CreateScope();
				IBookingService bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();

				Booking? booking = await bookingService.GetBooking(bookingId);
				if (booking?.Account?.Email is null) {
					continue;
				}
				Account account = booking.Account;

				string html = await bookingService.RenderTicket(bookingId);
				await emailerAdapter.SendEmailAsync("noreply@expoconnect.id", account.Email, "Test QR ticket", html);
				LoggerExtension.LogSuccessEmail(logger, account.Email);
			} catch (Exception ex) {
				logger.LogError("email error: {}", ex.Message);
			}
		}
	}
}