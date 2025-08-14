using Evently.Server.Common.Adapters.Data;
using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Domains.Models;
using Evently.Server.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Evently.Server.Features.Gatherings.Services;

public sealed class GatheringService(AppDbContext db) : IGatheringService {
	public async Task<Gathering?> GetGathering(long gatheringId) {
		return await db.Gatherings
			.Include((gathering) => gathering.Account)
			.Include(gathering => gathering.Bookings)
			.FirstOrDefaultAsync((gathering) => gathering.GatheringId == gatheringId);
	}

	public async Task<PageResult<Gathering>> GetGatherings(
		string? guestUserId,
		string? hostUserId,
		string? name,
		DateTimeOffset? startDate,
		DateTimeOffset? endDate,
		int? offset,
		int? limit) {
		IQueryable<Gathering> query = db.Gatherings
			.Where((gathering) => name == null || EF.Functions.ILike(gathering.Name, $"%{name}%"))
			.Where((gathering) =>
				guestUserId == null || gathering.Bookings.Any((be) => be.AccountId == guestUserId))
			.Where((gathering) => startDate == null || gathering.End >= startDate)
			.Where((gathering) => endDate == null || gathering.Start <= endDate)
			.Where((gathering) => hostUserId == null || gathering.HostId == hostUserId)
			.Include((gathering) => gathering.Account);

		int totalCount = await query.CountAsync();

		List<Gathering> gatherings = await query
			.OrderBy(gathering => gathering.Start)
			.Skip(offset ?? 0)
			.Take(limit ?? int.MaxValue)
			.Select((gathering) => gathering)
			.ToListAsync();

		return new PageResult<Gathering> {
			Items = gatherings,
			TotalCount = totalCount,
		};
	}

	public async Task<Gathering> CreateGathering(GatheringReqDto gatheringReqDto) {
		Gathering gathering = gatheringReqDto.ToGathering();
		db.Gatherings.Add(gathering);
		await db.SaveChangesAsync();
		return gathering;
	}

	public async Task<Gathering> UpdateGathering(long gatheringId, GatheringReqDto gatheringReqDto) {
		Gathering gathering = gatheringReqDto.ToGathering();
		Gathering current = await db.Gatherings.AsTracking()
			                    .FirstOrDefaultAsync((ex) => ex.GatheringId == gatheringId)
		                    ?? throw new KeyNotFoundException($"{gatheringId} not found");

		current.Name = gathering.Name;
		current.Description = gathering.Description;
		current.Start = gathering.Start;
		current.End = gathering.End;
		current.Location = gathering.Location;
		current.CoverSrc = gathering.CoverSrc;

		await db.SaveChangesAsync();
		return current;
	}

	public async Task DeleteGathering(long gatheringId) {
		Gathering gathering = await db.Gatherings
			.AsTracking()
			.SingleAsync((gathering) => gathering.GatheringId == gatheringId);
		db.Remove(gathering);
		await db.SaveChangesAsync();
	}
}