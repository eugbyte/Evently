using Evently.Server.Common.Adapters.Data;
using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Domains.Models;
using Evently.Server.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Evently.Server.Features.Members.Services;

public sealed class MemberService(AppDbContext db) : IMemberService {
	public async Task<PageResult<Member>> GetMembers(string? company, int? offset, int? limit) {
		IQueryable<Member> query = db.Members
			.Include((attendee) => attendee.MemberCategoryDetails)
			.Where((attendee) => company == null || EF.Functions.ILike(attendee.Company, $"%{company}%"));

		int totalCount = await query.CountAsync();

		List<Member> attendees = await query
			.OrderBy((attendee) => attendee.Id)
			.Skip(offset ?? 0)
			.Take(limit ?? int.MaxValue)
			.ToListAsync();

		return new PageResult<Member> {
			Items = attendees,
			TotalCount = totalCount,
		};
	}

	public async Task<Member?> GetMember(long memberId) {
		IQueryable<Member> query = db.Members
			.Include((attendee) => attendee.MemberCategoryDetails)
			.Where((a) => a.Id == memberId);
		return await query.FirstOrDefaultAsync();
	}

	public async Task<Member> CreateMember(MemberDto memberDto) {
		Member member = memberDto.ToMember();

		db.Members.Add(member);
		await db.SaveChangesAsync();
		return member;
	}

	public async Task<Member> UpdateMember(long memberId, MemberDto memberDto) {
		Member member = memberDto.ToMember();
		// need to include related entities intended to update - otherwise EF Core won't track these related entities and won't mark them as track
		// ignore connectionEvents and bookingEvents as those related entities should be updated via their respective services
		Member current = await db.Members.AsTracking()
			                 .Include((a) => a.MemberCategoryDetails)
			                 .ThenInclude(detail => detail.Category)
			                 .FirstOrDefaultAsync((a) => a.Id == memberId)
		                 ?? throw new KeyNotFoundException($"{member.Id} not found");

		current.Name = member.Name;
		current.Email = member.Email;
		current.Phone = member.Phone;
		current.Company = member.Company;
		current.Role = member.Role;
		current.Objective = member.Objective;
		current.AdSource = member.AdSource;
		current.MemberCategoryDetails = member.MemberCategoryDetails;
		current.LogoSrc = member.LogoSrc;

		await db.SaveChangesAsync();
		return current;
	}

	public async Task<Member> DeleteUser(long memberId) {
		Member member = await db.Members.AsTracking()
			.Where((attendee) => attendee.Id == memberId)
			.SingleAsync();
		db.Remove(member);
		return member;
	}
}