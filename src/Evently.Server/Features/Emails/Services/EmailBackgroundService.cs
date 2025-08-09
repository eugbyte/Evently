using Evently.Server.Domains.Interfaces;
using Evently.Server.Domains.Models;
using System.Threading.Channels;
using LoggerExtension=Evently.Server.Common.Extensions.LoggerExtension;

namespace Evently.Server.Features.Emails.Services;

public class EmailBackgroundService(
	ChannelReader<EmailMqPayload> reader,
	IEmailer emailer,
	ILogger<EmailBackgroundService> logger) : BackgroundService {
	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		while (await reader.WaitToReadAsync(stoppingToken)) {
			try {
				EmailMqPayload info = await reader.ReadAsync(stoppingToken);
				(string email, string html) = info;
				await emailer.SendEmailAsync("noreply@expoconnect.id", email, "Test QR ticket", html);
				LoggerExtension.LogSuccessEmail(logger, email);
			} catch (Exception ex) {
				logger.LogError("email error: {}", ex.Message);
			}
		}
	}
}