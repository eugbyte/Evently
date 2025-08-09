namespace Evently.Server.Common.Domains.Models;

public record BookingDto(
	string BookingId,
	long AttendeeId,
	long ExhibitionId,
	DateTimeOffset RegistrationDateTime,
	DateTimeOffset? CheckInDateTime,
	DateTimeOffset? CheckoutDateTime,
	DateTimeOffset? CancellationDateTime);