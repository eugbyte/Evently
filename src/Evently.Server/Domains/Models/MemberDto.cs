namespace Evently.Server.Domains.Models;

public sealed record MemberDto(
	long MemberId,
	string Name,
	string Email,
	string Phone,
	string Company,
	string Role,
	string Objective,
	string AdSource,
	string? LogoSrc,
	List<MemberCategoryDetailDto> AttendeeTopicDetails);