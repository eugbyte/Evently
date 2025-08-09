using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Domains.Models;
using Evently.Server.Common.Extensions;
using Evently.Server.Features.Auths.Services;
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
	[HttpGet("{memberId:long}", Name = "GetAttendee")]
	public async Task<ActionResult<Member>> GetAttendee(long memberId) {
		Member? attendee = await memberService.GetMember(memberId);
		if (attendee is null) {
			return NotFound();
		}

		return Ok(attendee);
	}

	[HttpGet("", Name = "GetAttendees")]
	public async Task<ActionResult<Member>> GetAttendees(string? company, int? offset,
		int? limit) {
		PageResult<Member> result = await memberService.GetMembers(company, offset, limit);
		List<Member> attendees = result.Items;
		int total = result.TotalCount;
		HttpContext.Response.Headers.Append("Access-Control-Expose-Headers", "X-Total-Count");
		HttpContext.Response.Headers.Append("X-Total-Count", value: total.ToString(CultureInfo.InvariantCulture));
		return Ok(attendees);
	}

	[HttpPost("", Name = "CreateAttendee")]
	public async Task<ActionResult<Member>> CreateAttendee(MemberDto memberDto) {
		memberDto = memberDto with { MemberId = 0 };
		ValidationResult result = await validator.ValidateAsync(memberDto.ToMember());
		if (!result.IsValid) {
			return BadRequest(result.Errors);
		}

		AuthenticateResult authResult = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
		if (!authResult.Succeeded) {
			return Unauthorized();
		}

		Member member = await memberService.CreateMember(memberDto);
		return Ok(member);
	}

	[HttpPut("{memberId:long}", Name = "UpdateAttendee")]
	public async Task<ActionResult<Member>> UpdateAttendee(long memberId, [FromBody] MemberDto memberDto) {
		Member? attendee = await memberService.GetMember(memberId);
		if (attendee is null) {
			return NotFound();
		}

		ValidationResult result = await validator.ValidateAsync(memberDto.ToMember());
		if (!result.IsValid) {
			return BadRequest(result.Errors);
		}

		if (!await this.IsResourceOwner(attendee.Id)) {
			return Forbid();
		}

		attendee = await memberService.UpdateMember(memberId, memberDto);
		return Ok(attendee);
	}
}