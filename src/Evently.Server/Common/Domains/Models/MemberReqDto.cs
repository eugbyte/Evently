namespace Evently.Server.Common.Domains.Models;

public sealed record MemberReqDto(
	string Id,
	string Name,
	string Email,
	string? LogoSrc);