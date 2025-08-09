namespace Evently.Server.Common.Domains.Models;

public sealed record GatheringDto(
	long GatheringId,
	string Name,
	string Description,
	DateTimeOffset Start,
	DateTimeOffset End,
	string Location,
	long EventOrganiserId,
	string? CoverSrc
);