using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Models;
using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Common.Domains.Interfaces;

public interface IMemberService {
	Task<Member?> GetMember(string memberId);
	Task<PageResult<Member>> GetMembers(string? name, int? offset, int? limit);
	Task<Member> CreateMember(MemberReqDto memberReqDto);
	Task<Member> UpdateMember(string memberId, MemberReqDto memberReqDto);

	[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
	[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
	Task<Member> DeleteUser(string memberId);
}