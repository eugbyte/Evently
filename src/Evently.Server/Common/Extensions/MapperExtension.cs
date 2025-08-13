using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Models;

namespace Evently.Server.Common.Extensions;

public static class MapperExtension {
	public static MemberReqDto ToMemberDto(this Member member) {
		MemberReqDto reqDto = new(
			member.MemberId,
			member.Name,
			member.Email,
			member.LogoSrc
		);
		return reqDto;
	}

	public static Member ToMember(this MemberReqDto memberReqDto) {
		Member member = new() {
			MemberId = memberReqDto.MemberId,
			Name = memberReqDto.Name,
			Email = memberReqDto.Email,
			LogoSrc = memberReqDto.LogoSrc,
		};
		return member;
	}

	public static Gathering ToGathering(this GatheringReqDto gatheringReqDto) {
		Gathering gathering = new() {
			GatheringId = gatheringReqDto.GatheringId,
			Name = gatheringReqDto.Name,
			Description = gatheringReqDto.Description,
			Start = gatheringReqDto.Start,
			End = gatheringReqDto.End,
			Location = gatheringReqDto.Location,
			OrganiserId = gatheringReqDto.EventOrganiserId,
			CoverSrc = gatheringReqDto.CoverSrc,
		};
		return gathering;
	}

	public static GatheringReqDto ToGatheringDto(this Gathering gathering) {
		GatheringReqDto reqDto = new(
			gathering.GatheringId,
			gathering.Name,
			gathering.Description,
			gathering.Start,
			gathering.End,
			gathering.Location,
			gathering.OrganiserId,
			gathering.CoverSrc
		);
		return reqDto;
	}


	public static Booking ToBooking(this BookingReqDto bookingReqDto) {
		return new Booking {
			BookingId = bookingReqDto.BookingId,
			MemberId = bookingReqDto.AttendeeId,
			GatheringId = bookingReqDto.ExhibitionId,
			RegistrationDateTime = bookingReqDto.RegistrationDateTime,
			CheckInDateTime = bookingReqDto.CheckInDateTime,
			CheckoutDateTime = bookingReqDto.CheckoutDateTime,
			CancellationDateTime = bookingReqDto.CancellationDateTime,
		};
	}
}