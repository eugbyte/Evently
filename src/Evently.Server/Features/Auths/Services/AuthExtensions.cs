using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Evently.Server.Features.Auths.Services;

public static class AuthExtensions {
	public static async Task<bool> IsResourceOwner(this ControllerBase controller, long? resourceIdentityUserId) {
		IAuthorizationService authorizationService =
			controller.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();

		AuthenticateResult authenticationResult =
			await controller.HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
		if (!authenticationResult.Succeeded || resourceIdentityUserId is null) {
			return false;
		}

		ClaimsPrincipal principal = authenticationResult.Principal ?? new ClaimsPrincipal();
		AuthorizationResult authorizationResult =
			await authorizationService.AuthorizeAsync(principal,
				resourceIdentityUserId,
				SameUserRequirement.PolicyName);
		return authorizationResult.Succeeded;
	}

	public static async Task<IdentityUser?> FindByClaimsPrincipalAsync(this UserManager<IdentityUser> userManager,
		ClaimsPrincipal? claimsPrincipal) {
		claimsPrincipal ??= new ClaimsPrincipal();
		string loginProvider = claimsPrincipal.Identity?.AuthenticationType ?? string.Empty;
		string providerKey = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

		IdentityUser? user = await userManager.GetUserAsync(claimsPrincipal);
		return user ?? await userManager.FindByLoginAsync(loginProvider, providerKey);
	}
}