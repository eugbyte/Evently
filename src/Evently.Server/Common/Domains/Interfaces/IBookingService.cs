using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Models;

namespace Evently.Server.Common.Domains.Interfaces;

public interface IBookingService {
	Task<Booking?> GetBooking(string bookingId);

	Task<PageResult<Booking>> GetBookings(long? guestMemberId, long? hostMemberId, DateTime? checkInStart,
		DateTime? checkInEnd,
		bool? isCancelled, int? offset, int? limit);

	Task<Booking> CreateBooking(BookingDto bookingDto);
	Task<Booking> UpdateBooking(string bookingId, BookingDto bookingDto);
	Task<bool> Exists(string bookingId);
	Task<string> RenderMemberTicket(string bookingId);
}