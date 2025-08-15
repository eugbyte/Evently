using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Models;

namespace Evently.Server.Common.Domains.Interfaces;

public interface IGatheringService {
	Task<Gathering?> GetGathering(long gatheringId);

	Task<PageResult<Gathering>> GetGatherings(string? attendeeId, string? organiserId,
		string? exhibitionName,
		DateTimeOffset? startDate, DateTimeOffset? endDate,
		int? offset,
		int? limit);

	Task<Gathering> CreateGathering(GatheringReqDto gatheringReqDto);
	Task<Gathering> UpdateGathering(long gatheringId, GatheringReqDto gatheringReqDto);
	Task DeleteGathering(long gatheringId);
}