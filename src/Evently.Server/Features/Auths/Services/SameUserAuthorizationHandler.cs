using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Evently.Server.Features.Auths.Services;

public sealed class SameUserAuthorizationHandler(UserManager<IdentityUser> userManager)
	: AuthorizationHandler<SameUserRequirement, string> {
	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		SameUserRequirement requirement,
		string? identityUserId) {
		ClaimsPrincipal principal = context.User;
		IdentityUser? user = await userManager.FindByClaimsPrincipalAsync(principal);

		bool userMatch = user is not null && user.Id == identityUserId;
		if (userMatch) {
			context.Succeed(requirement);
		}
	}
}

public class SameUserRequirement : IAuthorizationRequirement {
	public const string PolicyName = "SameUserPolicy";
}