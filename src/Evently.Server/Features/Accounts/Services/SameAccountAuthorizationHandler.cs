using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Evently.Server.Features.Accounts.Services;

// Based on https://tinyurl.com/5cxw9vmu
public sealed class SameAccountAuthorizationHandler(UserManager<IdentityUser> userManager)
	: AuthorizationHandler<SameUserRequirement, string> {
	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		SameUserRequirement requirement,
		string? identityUserId) {
		ClaimsPrincipal principal = context.User;
		IdentityUser? user = await FindByClaimsPrincipalAsync(userManager, principal);

		bool userMatch = user is not null && user.Id == identityUserId;
		if (userMatch) {
			context.Succeed(requirement);
		}
	}

	private static async Task<IdentityUser?> FindByClaimsPrincipalAsync(UserManager<IdentityUser> userManager,
		ClaimsPrincipal claimsPrincipal) {
		string loginProvider = claimsPrincipal.Identity?.AuthenticationType ?? string.Empty;
		string providerKey = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

		IdentityUser? user = await userManager.GetUserAsync(claimsPrincipal);
		return user ?? await userManager.FindByLoginAsync(loginProvider, providerKey);
	}
}

public class SameUserRequirement : IAuthorizationRequirement {
	public const string PolicyName = "SameUserPolicy";
}