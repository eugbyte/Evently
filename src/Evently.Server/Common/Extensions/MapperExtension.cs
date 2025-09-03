using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Models;

namespace Evently.Server.Common.Extensions;

public static class MapperExtension {
	public static Gathering ToGathering(this GatheringReqDto gatheringReqDto) {
		List<GatheringCategoryDetail> gatheringCategoryDetails = gatheringReqDto.GatheringCategoryDetails
			.Select((detail) => new GatheringCategoryDetail {
				GatheringId = gatheringReqDto.GatheringId,
				CategoryId = detail.CategoryId,
			})
			.ToList();
		Gathering gathering = new() {
			GatheringId = gatheringReqDto.GatheringId,
			Name = gatheringReqDto.Name,
			Description = gatheringReqDto.Description,
			Start = gatheringReqDto.Start,
			End = gatheringReqDto.End,
			CancellationDateTime = gatheringReqDto.CancellationDateTime,
			Location = gatheringReqDto.Location,
			OrganiserId = gatheringReqDto.OrganiserId,
			CoverSrc = gatheringReqDto.CoverSrc,
			GatheringCategoryDetails = gatheringCategoryDetails,
		};
		return gathering;
	}

	public static GatheringReqDto ToGatheringDto(this Gathering gathering) {
		List<GatheringCategoryDetailDto> gatheringCategoryDetails = gathering.GatheringCategoryDetails
			.Select(detail => new GatheringCategoryDetailDto(detail.GatheringId, detail.CategoryId))
			.ToList();
		GatheringReqDto reqDto = new(
			gathering.GatheringId,
			gathering.Name,
			gathering.Description,
			gathering.Start,
			gathering.End,
			gathering.CancellationDateTime,
			gathering.Location,
			gathering.OrganiserId,
			gathering.CoverSrc,
			gatheringCategoryDetails
		);
		return reqDto;
	}


	public static Booking ToBooking(this BookingReqDto bookingReqDto) {
		return new Booking {
			BookingId = bookingReqDto.BookingId,
			AttendeeId = bookingReqDto.AttendeeId,
			GatheringId = bookingReqDto.GatheringId,
			CreationDateTime = bookingReqDto.CreationDateTime,
			CheckInDateTime = bookingReqDto.CheckInDateTime,
			CheckoutDateTime = bookingReqDto.CheckoutDateTime,
			CancellationDateTime = bookingReqDto.CancellationDateTime,
		};
	}

	public static BookingReqDto ToBookingDto(this Booking booking) {
		return new BookingReqDto(
			booking.BookingId,
			booking.AttendeeId,
			booking.GatheringId,
			booking.CreationDateTime,
			booking.CheckInDateTime,
			booking.CheckoutDateTime,
			booking.CancellationDateTime
		);
	}

	public static AccountDto ToAccountDto(this Account account) {
		return new AccountDto(
			account.Id,
			Email: account.Email ?? string.Empty,
			Username: account.UserName ?? string.Empty,
			Name: account.Name
		);
	}
}