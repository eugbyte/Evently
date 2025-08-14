namespace Evently.Server.Common.Domains.Models;

public sealed record BookingReqDto(
	string BookingId,
	string GuestId,
	long GatheringId,
	DateTimeOffset RegistrationDateTime,
	DateTimeOffset? CheckInDateTime,
	DateTimeOffset? CheckoutDateTime,
	DateTimeOffset? CancellationDateTime);