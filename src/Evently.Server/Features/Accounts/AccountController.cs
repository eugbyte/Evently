using Evently.Server.Common.Domains.Exceptions;
using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Domains.Models;
using Evently.Server.Common.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Evently.Server.Features.Accounts;

// Based on https://tinyurl.com/26arz8vk
[ApiController]
[Route("api/v1/auth/external")]
public sealed class AccountController(
	IAccountsService accountService,
	ILogger<AccountController> logger) : ControllerBase {

	private readonly Dictionary<string, string> _authSchemes = new() {
		{ "google", GoogleDefaults.AuthenticationScheme },
		{ "microsoft", MicrosoftAccountDefaults.AuthenticationScheme },
	};

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
			UserName: user.UserName ?? ""));
	}

	[HttpPost("logout")]
	public async Task<ActionResult> Logout(string? redirectUrl = "") {
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

	[HttpGet("{provider}/login")]
	public IActionResult Login(string provider, string? originUrl = "") {
		Uri rootUri = Request.RootUri();
		string uri = Url.Action("Callback", "Account") ?? "";
		UriBuilder combined = new(rootUri) {
			Path = uri,
			Query = $"originUrl={originUrl}",
		};

		logger.LogCallbackUrl(combined.Uri.AbsoluteUri);
		AuthenticationProperties properties = new() {
			RedirectUri = combined.Uri.AbsoluteUri,
			IsPersistent = true,
		};
		return Challenge(properties, _authSchemes.GetValueOrDefault(provider) ?? "");
	}

	[HttpGet("{provider}/callback")]
	public async Task<ActionResult<int>> Callback(string provider, [FromQuery] string originUrl = "") {
		AuthenticateResult result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

		if (!result.Succeeded || result.Principal is null) {
			return Unauthorized();
		}

		ClaimsPrincipal claimsPrincipal = result.Principal;
		if (claimsPrincipal == null) {
			throw new ExternalLoginProviderException(provider, "ClaimsPrincipal is null");
		}

		await accountService.ExternalLogin(claimsPrincipal, loginProvider: _authSchemes.GetValueOrDefault(provider) ?? "");
		return Redirect(originUrl);
	}
}