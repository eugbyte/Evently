namespace Evently.Server.Common.Domains.Models;

public sealed record BookingReqDto(
	string BookingId,
	long AttendeeId,
	long ExhibitionId,
	DateTimeOffset RegistrationDateTime,
	DateTimeOffset? CheckInDateTime,
	DateTimeOffset? CheckoutDateTime,
	DateTimeOffset? CancellationDateTime);