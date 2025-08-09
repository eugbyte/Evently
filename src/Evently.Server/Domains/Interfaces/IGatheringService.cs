using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Models;

namespace Evently.Server.Domains.Interfaces;

public interface IGatheringService {
	Task<Gathering?> GetGathering(long gatheringId);

	Task<PageResult<Gathering>> GetGatherings(long? guestUserId, long? hostUserId,
		string? exhibitionName,
		DateTimeOffset? startDate, DateTimeOffset? endDate,
		int? offset,
		int? limit);

	Task<Gathering> CreateGathering(GatheringDto gatheringDto);
	Task<Gathering> UpdateGathering(long gatheringId, GatheringDto gatheringDto);
	Task DeleteGathering(long gatheringId);
	Task<Dictionary<string, int>> GetCategoryCount(long gatheringId, int? offset, int? limit);
}