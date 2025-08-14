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
using MimeKit;
using System.Globalization;

namespace Evently.Server.Features.Gatherings;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class GatheringsController(
	IGatheringService gatheringService,
	IFileStorageService imageStorageService,
	IValidator<Gathering> validator) : ControllerBase {
	[HttpGet("{gatheringId:long}", Name = "GetGathering")]
	public async Task<ActionResult<Gathering>> GetGathering(long gatheringId) {
		Gathering? customer = await gatheringService.GetGathering(gatheringId);
		if (customer is null) {
			return NotFound();
		}

		return Ok(customer);
	}

	[HttpGet("", Name = "GetGatherings")]
	public async Task<ActionResult<List<Gathering>>> GetGatherings(string? guestId,
		string? hostId, string? exhibitionName,
		DateTimeOffset? start, DateTimeOffset? end, int? offset, int? limit) {
		PageResult<Gathering> result = await gatheringService.GetGatherings(guestId,
			hostId,
			exhibitionName,
			start,
			end,
			offset,
			limit);
		List<Gathering> exhibitions = result.Items;
		int total = result.TotalCount;
		HttpContext.Response.Headers.Append("Access-Control-Expose-Headers", "X-Total-Count");
		HttpContext.Response.Headers.Append("X-Total-Count", value: total.ToString(CultureInfo.InvariantCulture));
		return Ok(exhibitions);
	}

	[HttpPost("", Name = "CreateGathering")]
	public async Task<ActionResult<Gathering>> CreateGathering(GatheringReqDto gatheringReqDto) {
		gatheringReqDto = gatheringReqDto with { GatheringId = 0 };
		ValidationResult validationResult = await validator.ValidateAsync(gatheringReqDto.ToGathering());
		if (!validationResult.IsValid) {
			return BadRequest(validationResult.Errors);
		}

		AuthenticateResult authenticationResult =
			await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
		if (!authenticationResult.Succeeded) {
			return Unauthorized();
		}

		Gathering gathering = await gatheringService.CreateGathering(gatheringReqDto);
		return Ok(gathering);
	}

	[HttpPut("{gatheringId:long}", Name = "UpdateGathering")]
	public async Task<ActionResult> UpdateGathering(long gatheringId, [FromBody] GatheringReqDto gatheringReqDto) {
		Gathering? exhibition = await gatheringService.GetGathering(gatheringId);
		if (exhibition is null) {
			return NotFound();
		}

		ValidationResult result = await validator.ValidateAsync(gatheringReqDto.ToGathering());
		if (!result.IsValid) {
			return BadRequest(result.Errors);
		}

		if (!await this.IsResourceOwner(exhibition.Member?.Id)) {
			return Forbid();
		}

		exhibition = await gatheringService.UpdateGathering(gatheringId, gatheringReqDto);
		return Ok(exhibition);
	}

	[HttpDelete("{gatheringId:long}", Name = "DeleteGathering")]
	public async Task<ActionResult<Gathering>> DeleteGathering(long gatheringId) {
		Gathering? exhibition = await gatheringService.GetGathering(gatheringId);
		if (exhibition is null) {
			return NotFound();
		}

		if (!await this.IsResourceOwner(exhibition.Member?.Id)) {
			return Forbid();
		}

		await gatheringService.DeleteGathering(gatheringId);
		return NoContent();
	}

	[HttpPost("{gatheringId:long}/images", Name = "UploadImages")]
	public async Task<IActionResult> UploadCoverImage(long gatheringId, [FromForm] IFormFile? coverImg) {
		Gathering? gathering = await gatheringService.GetGathering(gatheringId);
		if (gathering is null) {
			return NotFound();
		}

		if (coverImg is not null) {
			string fileName = $"gatherings/{gatheringId}/cover-image{Path.GetExtension(coverImg.FileName)}";
			BinaryData binaryData = await coverImg.ToBinaryData();

			Uri uri = await imageStorageService.UploadFile(fileName,
				binaryData,
				mimeType: MimeTypes.GetMimeType(coverImg.FileName));
			gathering.CoverSrc = uri.AbsoluteUri;
		}

		gathering = await gatheringService.UpdateGathering(gatheringId, gatheringReqDto: gathering.ToGatheringDto());
		return Ok(new {
			coverUri = gathering.CoverSrc,
		});
	}
}