using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Domains.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using LoggerExtension=Evently.Server.Common.Extensions.LoggerExtension;

namespace Evently.Server.Features.Emails.Services;

public sealed class Emailer(ILogger<Emailer> logger, IOptions<Settings> settings) : IEmailer {
	private readonly string _from = settings.Value.EmailSettings.ActualFrom;
	private readonly string _password = settings.Value.EmailSettings.SmtpPassword;

	public async Task SendEmailAsync(string senderEmail, string recipientEmail, string subject, string body) {
		MimeMessage emailMessage = new();
		emailMessage.From.Add(new MailboxAddress(senderEmail, senderEmail));
		emailMessage.To.Add(new MailboxAddress(recipientEmail, recipientEmail));
		emailMessage.Subject = subject;

		BodyBuilder bodyBuilder = new() {
			HtmlBody = body,
		};

		emailMessage.Body = bodyBuilder.ToMessageBody();

		try {
			SmtpClient client = new();
			await client.ConnectAsync("smtp.gmail.com", port: 587, useSsl: false);
			// smtp auth
			await client.AuthenticateAsync(_from, _password);
			await client.SendAsync(emailMessage);
			await client.DisconnectAsync(true);
		} catch (Exception ex) {
			LoggerExtension.LogCallbackUrl(logger, ex.Message);
		}
	}
}