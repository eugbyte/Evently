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
			.Include((gathering) => gathering.Member)
			.Include(gathering => gathering.BookingEvents)
			.FirstOrDefaultAsync((gathering) => gathering.GatheringId == gatheringId);
	}

	public async Task<PageResult<Gathering>> GetGatherings(
		long? guestUserId,
		long? hostUserId,
		string? name,
		DateTimeOffset? startDate,
		DateTimeOffset? endDate,
		int? offset,
		int? limit) {
		IQueryable<Gathering> query = db.Gatherings
			.Where((gathering) => name == null || EF.Functions.ILike(gathering.Name, $"%{name}%"))
			.Where((gathering) =>
				guestUserId == null || gathering.BookingEvents.Any((be) => be.MemberId == guestUserId))
			.Where((gathering) => startDate == null || gathering.End >= startDate)
			.Where((gathering) => endDate == null || gathering.Start <= endDate)
			.Where((gathering) => hostUserId == null || gathering.MemberId == hostUserId)
			.Include((gathering) => gathering.Member)
			.Include((gathering) => gathering.BookingEvents);

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

	public async Task<Gathering> CreateGathering(GatheringDto gatheringDto) {
		Gathering gathering = gatheringDto.ToGathering();
		db.Gatherings.Add(gathering);
		await db.SaveChangesAsync();
		return gathering;
	}

	public async Task<Gathering> UpdateGathering(long gatheringId, GatheringDto gatheringDto) {
		Gathering gathering = gatheringDto.ToGathering();
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