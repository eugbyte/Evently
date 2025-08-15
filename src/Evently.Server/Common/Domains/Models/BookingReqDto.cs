namespace Evently.Server.Common.Domains.Models;

public sealed record BookingReqDto(
	string BookingId,
	string AttendeeId,
	long OrganiserId,
	bool IsOrganiser,
	DateTimeOffset RegistrationDateTime,
	DateTimeOffset? CheckInDateTime,
	DateTimeOffset? CheckoutDateTime,
	DateTimeOffset? CancellationDateTime);