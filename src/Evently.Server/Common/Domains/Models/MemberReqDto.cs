namespace Evently.Server.Common.Domains.Models;

public sealed record MemberReqDto(
	long MemberId,
	string Name,
	string Email,
	string? LogoSrc);