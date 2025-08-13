using Evently.Server.Common.Adapters.Data;
using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Domains.Models;
using Evently.Server.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Evently.Server.Features.Members.Services;

public sealed class MemberService(AppDbContext db) : IMemberService {
	public async Task<PageResult<Member>> GetMembers(string? name, int? offset, int? limit) {
		IQueryable<Member> query = db.Members
			.Where((member) => name == null || EF.Functions.ILike(member.Name, $"%{name}%"));

		int totalCount = await query.CountAsync();

		List<Member> members = await query
			.OrderBy((member) => member.MemberId)
			.Skip(offset ?? 0)
			.Take(limit ?? int.MaxValue)
			.ToListAsync();

		return new PageResult<Member> {
			Items = members,
			TotalCount = totalCount,
		};
	}

	public async Task<Member?> GetMember(long memberId) {
		IQueryable<Member> query = db.Members
			.Where((a) => a.MemberId == memberId);
		return await query.FirstOrDefaultAsync();
	}

	public async Task<Member> CreateMember(MemberReqDto memberReqDto) {
		Member member = memberReqDto.ToMember();

		db.Members.Add(member);
		await db.SaveChangesAsync();
		return member;
	}

	public async Task<Member> UpdateMember(long memberId, MemberReqDto memberReqDto) {
		Member member = memberReqDto.ToMember();
		// need to include related entities intended to update - otherwise EF Core won't track these related entities and won't mark them as track
		// ignore connectionEvents and bookingEvents as those related entities should be updated via their respective services
		Member current = await db.Members.AsTracking()
			                 .FirstOrDefaultAsync((a) => a.MemberId == memberId)
		                 ?? throw new KeyNotFoundException($"{member.MemberId} not found");

		current.Name = member.Name;
		current.Email = member.Email;
		current.LogoSrc = member.LogoSrc;

		await db.SaveChangesAsync();
		return current;
	}

	public async Task<Member> DeleteUser(long memberId) {
		Member member = await db.Members
			.AsTracking()
			.SingleAsync((member) => member.MemberId == memberId);
		db.Remove(member);
		return member;
	}
}