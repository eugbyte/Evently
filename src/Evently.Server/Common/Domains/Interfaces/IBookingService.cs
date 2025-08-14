using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Models;

namespace Evently.Server.Common.Domains.Interfaces;

public interface IBookingService {
	Task<Booking?> GetBooking(string bookingId);

	Task<PageResult<Booking>> GetBookings(string? guestId, long? gatheringId, DateTime? checkInStart,
		DateTime? checkInEnd,
		bool? isCancelled, int? offset, int? limit);

	Task<Booking> CreateBooking(BookingReqDto bookingReqDto);
	Task<Booking> UpdateBooking(string bookingId, BookingReqDto bookingReqDto);
	Task<bool> Exists(string bookingId);
	Task<string> RenderTicket(string bookingId);
}