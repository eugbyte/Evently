﻿namespace Evently.Server.Common.Extensions;

public static partial class LoggerExtension {
	[LoggerMessage(
		EventId = 1,
		Level = LogLevel.Information,
		Message = "{key}: {value}")]
	public static partial void LogValue(
		this ILogger logger, string key, string? value);

	[LoggerMessage(
		EventId = 2,
		Level = LogLevel.Information,
		Message = "Callback Url: {callbackUrl}")]
	public static partial void LogCallbackUrl(
		this ILogger logger, string? callbackUrl);

	[LoggerMessage(
		EventId = 3,
		Level = LogLevel.Information,
		Message = "Email sent successfully to {email}")]
	public static partial void LogSuccessEmail(
		this ILogger logger, string? email);

	[LoggerMessage(
		EventId = 4,
		Level = LogLevel.Error,
		Message = "Error occurred at {context}: {email}")]
	public static partial void LogErrorContext(
		this ILogger logger, string context, string email);
}