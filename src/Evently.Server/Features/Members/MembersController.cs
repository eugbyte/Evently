using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Domains.Models;
using Evently.Server.Common.Extensions;
using Evently.Server.Features.Accounts.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Evently.Server.Features.Members;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class MembersController(IMemberService memberService, IValidator<Member> validator)
	: ControllerBase {
	[HttpGet("{memberId:long}", Name = "GetMember")]
	public async Task<ActionResult<Member>> GetMember(long memberId) {
		Member? member = await memberService.GetMember(memberId);
		if (member is null) {
			return NotFound();
		}

		return Ok(member);
	}

	[HttpGet("", Name = "GetMembers")]
	public async Task<ActionResult<Member>> GetMembers(string? name, int? offset,
		int? limit) {
		PageResult<Member> result = await memberService.GetMembers(name, offset, limit);
		List<Member> attendees = result.Items;
		int total = result.TotalCount;
		HttpContext.Response.Headers.Append("Access-Control-Expose-Headers", "X-Total-Count");
		HttpContext.Response.Headers.Append("X-Total-Count", value: total.ToString(CultureInfo.InvariantCulture));
		return Ok(attendees);
	}

	[HttpPost("", Name = "CreateMember")]
	public async Task<ActionResult<Member>> CreateMember(MemberReqDto memberReqDto) {
		memberReqDto = memberReqDto with { MemberId = 0 };
		ValidationResult result = await validator.ValidateAsync(memberReqDto.ToMember());
		if (!result.IsValid) {
			return BadRequest(result.Errors);
		}

		AuthenticateResult authResult = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
		if (!authResult.Succeeded) {
			return Unauthorized();
		}

		Member member = await memberService.CreateMember(memberReqDto);
		return Ok(member);
	}

	[HttpPut("{memberId:long}", Name = "UpdateMember")]
	public async Task<ActionResult<Member>> UpdateMember(long memberId, [FromBody] MemberReqDto memberReqDto) {
		Member? member = await memberService.GetMember(memberId);
		if (member is null) {
			return NotFound();
		}

		ValidationResult result = await validator.ValidateAsync(memberReqDto.ToMember());
		if (!result.IsValid) {
			return BadRequest(result.Errors);
		}

		if (!await this.IsResourceOwner(member.MemberId)) {
			return Forbid();
		}

		member = await memberService.UpdateMember(memberId, memberReqDto);
		return Ok(member);
	}
}