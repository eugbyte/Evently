using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Models;

namespace Evently.Server.Common.Extensions;

public static class MapperExtension {
	public static MemberDto ToMemberDto(this Member member) {
		MemberDto dto = new(
			member.Id,
			member.Name,
			Email: member.Email ?? "",
			member.Phone,
			member.Company,
			member.Role,
			member.Objective,
			member.AdSource,
			member.LogoSrc,
			AttendeeTopicDetails: member.MemberCategoryDetails
				.Select((detail) => new MemberCategoryDetailDto(detail.MemberId, detail.CategoryId)).ToList()
		);
		return dto;
	}

	public static Member ToMember(this MemberDto memberDto) {
		Member member = new() {
			Id = memberDto.MemberId,
			Name = memberDto.Name,
			Email = memberDto.Email,
			Phone = memberDto.Phone,
			Company = memberDto.Company,
			Role = memberDto.Role,
			Objective = memberDto.Objective,
			AdSource = memberDto.AdSource,
			LogoSrc = memberDto.LogoSrc,
			MemberCategoryDetails = memberDto.AttendeeTopicDetails.Select((dto) => new MemberCategoryDetail {
				MemberId = dto.MemberId,
				CategoryId = dto.CategoryId,
			}).ToList(),
		};
		return member;
	}

	public static Gathering ToGathering(this GatheringDto gatheringDto) {
		Gathering gathering = new() {
			GatheringId = gatheringDto.GatheringId,
			Name = gatheringDto.Name,
			Description = gatheringDto.Description,
			Start = gatheringDto.Start,
			End = gatheringDto.End,
			Location = gatheringDto.Location,
			MemberId = gatheringDto.EventOrganiserId,
			CoverSrc = gatheringDto.CoverSrc,
		};
		return gathering;
	}

	public static GatheringDto ToGatheringDto(this Gathering gathering) {
		GatheringDto dto = new(
			gathering.GatheringId,
			gathering.Name,
			gathering.Description,
			gathering.Start,
			gathering.End,
			gathering.Location,
			gathering.MemberId,
			gathering.CoverSrc
		);
		return dto;
	}


	public static Booking ToBooking(this BookingDto bookingDto) {
		return new Booking {
			BookingId = bookingDto.BookingId,
			MemberId = bookingDto.AttendeeId,
			GatheringId = bookingDto.ExhibitionId,
			RegistrationDateTime = bookingDto.RegistrationDateTime,
			CheckInDateTime = bookingDto.CheckInDateTime,
			CheckoutDateTime = bookingDto.CheckoutDateTime,
			CancellationDateTime = bookingDto.CancellationDateTime,
		};
	}
}