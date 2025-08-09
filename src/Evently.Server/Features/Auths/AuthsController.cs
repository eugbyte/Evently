using Evently.Server.Common.Domains.Exceptions;
using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Evently.Server.Features.Auths;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class AuthsController(
	IAccountsService accountService,
	ILogger<AuthsController> logger) : ControllerBase {
	[HttpGet("account", Name = "Get Account")]
	public async Task<ActionResult> GetAccount() {
		AuthenticateResult result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
		if (!result.Succeeded) {
			return Unauthorized();
		}

		ClaimsPrincipal principal = result.Principal ?? new ClaimsPrincipal();
		IdentityUser? user = await accountService.FindByClaimsPrincipalAsync(principal);
		if (user is null) {
			return NotFound(new { message = "User not found" });
		}

		return Ok(new Account(
			user.Id,
			Email: user.Email ?? "",
			UserName: user.UserName ?? "",
			PhoneNumber: user.PhoneNumber ?? ""));
	}

	[HttpPost("external/logout")]
	public async Task<ActionResult> Logout([FromQuery] string? redirectUrl = "") {
		// Sign out of an external identity provider (if used)
		AuthenticationProperties authProps = new() {
			RedirectUri = redirectUrl,
			IsPersistent = true,
		};
		await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme, authProps);
		// Manually remove the authentication cookie
		// Delete each cookie
		foreach (string cookieName in Request.Cookies.Keys) {
			Response.Cookies.Delete(cookieName);
		}

		return Ok(new { redirectUrl });
	}

	[HttpGet("google/login")]
	public IActionResult GoogleLogin([FromQuery] string? originUrl = "") {
		Uri rootUri = Request.RootUri();
		string uri = Url.Action("GoogleCallback", "Auths") ?? "";
		UriBuilder combined = new(rootUri) {
			Path = uri,
			Query = $"originUrl={originUrl}",
		};

		logger.LogCallbackUrl(combined.Uri.AbsoluteUri);
		AuthenticationProperties properties = new() {
			RedirectUri = combined.Uri.AbsoluteUri,
			IsPersistent = true,
		};
		return Challenge(properties, GoogleDefaults.AuthenticationScheme);
	}

	[HttpGet("google/callback")]
	public async Task<ActionResult<int>> GoogleCallback([FromQuery] string originUrl = "") {
		AuthenticateResult result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

		if (!result.Succeeded || result.Principal is null) {
			return Unauthorized();
		}

		ClaimsPrincipal claimsPrincipal = result.Principal;
		if (claimsPrincipal == null) {
			throw new ExternalLoginProviderException("Google", "ClaimsPrincipal is null");
		}

		await accountService.ExternalLogin(claimsPrincipal, "Google");
		return Redirect(originUrl);
	}

	[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Local")]
	private sealed record Account(string IdentityUserId, string Email, string UserName, string PhoneNumber);
}