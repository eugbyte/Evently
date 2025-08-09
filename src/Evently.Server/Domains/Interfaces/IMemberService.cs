using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Models;
using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Domains.Interfaces;

public interface IMemberService {
	Task<Member?> GetMember(long memberId);
	Task<PageResult<Member>> GetMembers(string? company, int? offset, int? limit);
	Task<Member> CreateMember(MemberDto memberDto);
	Task<Member> UpdateMember(long memberId, MemberDto memberDto);

	[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
	[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
	Task<Member> DeleteUser(long memberId);
}