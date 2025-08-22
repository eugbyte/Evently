namespace Evently.Server.Common.Domains.Models;

public sealed record GatheringReqDto(
	long GatheringId,
	string Name,
	string Description,
	DateTimeOffset Start,
	DateTimeOffset End,
	DateTimeOffset? CancellationDateTime,
	string Location,
	string OrganiserId,
	string? CoverSrc
);