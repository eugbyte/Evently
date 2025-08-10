using Evently.Server.Common.Domains.Exceptions;
using Evently.Server.Common.Domains.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Evently.Server.Features.Accounts.Services;

// Based on https://tinyurl.com/4u4r7ywy
public sealed partial class AccountService(UserManager<IdentityUser> userManager) : IAccountsService {
	public async Task<IdentityUser> ExternalLogin(ClaimsPrincipal claimsPrincipal, string loginProvider) {
		string providerKey = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
		UserLoginInfo info = new(loginProvider, providerKey, loginProvider);

		IdentityUser user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey)
		                    ?? await CreateExternalUser(claimsPrincipal, loginProvider);
		await userManager.AddLoginAsync(user, info);
		return user;
	}

	public async Task<IdentityUser?> FindByClaimsPrincipalAsync(ClaimsPrincipal claimsPrincipal) {
		string loginProvider = claimsPrincipal.Identity?.AuthenticationType ?? string.Empty;
		string providerKey = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

		IdentityUser? user = await userManager.GetUserAsync(claimsPrincipal);
		return user ?? await userManager.FindByLoginAsync(loginProvider, providerKey);
	}

	private async Task<IdentityUser> CreateExternalUser(ClaimsPrincipal claimsPrincipal, string loginProvider) {
		UserLoginInfo info = new(loginProvider,
			providerKey: claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
			loginProvider);
		string? email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
		if (email == null) {
			throw new ExternalLoginProviderException(loginProvider, "Email is null");
		}

		string username = claimsPrincipal.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
		username = UsernameRegex().Replace(username, "");

		IdentityUser newUser = new() {
			UserName = username,
			Email = email,
			EmailConfirmed = true,
		};

		IdentityResult result = await userManager.CreateAsync(newUser);

		if (!result.Succeeded) {
			throw new ExternalLoginProviderException(loginProvider,
				message: $"Unable to create user: {string.Join(", ",
					values: result.Errors.Select(x => x.Description))}");
		}

		IdentityResult loginResult = await userManager.AddLoginAsync(newUser, info);
		if (!loginResult.Succeeded) {
			throw new ExternalLoginProviderException(loginProvider,
				message: $"Unable to login user: {string.Join(", ",
					values: loginResult.Errors.Select(err => err.Description))}");
		}

		return newUser;
	}

	[GeneratedRegex("[^a-zA-Z0-9]")]
	private static partial Regex UsernameRegex();
}