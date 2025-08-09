using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Evently.Server.Common.Domains.Interfaces;

public interface IAccountsService {
	Task<IdentityUser> ExternalLogin(ClaimsPrincipal claimsPrincipal, string loginProvider);
	Task<IdentityUser?> FindByClaimsPrincipalAsync(ClaimsPrincipal claimsPrincipal);
}