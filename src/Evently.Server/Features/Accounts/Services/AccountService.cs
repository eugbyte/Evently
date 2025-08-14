using Evently.Server.Common.Adapters.Data;
using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Exceptions;
using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Domains.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Evently.Server.Features.Accounts.Services;

// Based on https://tinyurl.com/4u4r7ywy
public sealed partial class AccountService(UserManager<Account> userManager, AppDbContext db) : IAccountsService {
	public async Task<Account> ExternalLogin(ClaimsPrincipal claimsPrincipal, string loginProvider) {
		string providerKey = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
		UserLoginInfo info = new(loginProvider, providerKey, loginProvider);

		Account user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey)
		               ?? await CreateExternalUser(claimsPrincipal, loginProvider);
		await userManager.AddLoginAsync(user, info);
		return user;
	}

	public async Task<Account?> FindByClaimsPrincipalAsync(ClaimsPrincipal claimsPrincipal) {
		string loginProvider = claimsPrincipal.Identity?.AuthenticationType ?? string.Empty;
		string providerKey = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

		Account? user = await userManager.GetUserAsync(claimsPrincipal);
		return user ?? await userManager.FindByLoginAsync(loginProvider, providerKey);
	}

	private async Task<Account> CreateExternalUser(ClaimsPrincipal claimsPrincipal, string loginProvider) {
		UserLoginInfo info = new(loginProvider,
			providerKey: claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
			loginProvider);
		string? email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
		if (email == null) {
			throw new ExternalLoginProviderException(loginProvider, "Email is null");
		}

		string username = claimsPrincipal.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
		username = UsernameRegex().Replace(username, "");

		Account newUser = new() {
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
	public async Task<List<Account>> GetAccounts(string? email, string? name, string? username) {
		IQueryable<Account> query = db.Accounts
			.Where(account => email == null || EF.Functions.ILike(account.Email ?? "", $"%{email}%"))
			.Where(account => username == null || EF.Functions.ILike(account.UserName ?? "", $"%{username}%"))
			.Where(account => name == null || EF.Functions.ILike(account.Name, $"%{name}%"));

		List<Account> members = await query
			.OrderBy((member) => member.Id)
			.ToListAsync();
		return members;
	}

	public async Task<Account?> GetAccount(string memberId) {
		return await userManager.FindByIdAsync(memberId);
	}

	public async Task<Account> UpdateAccount(string memberId, AccountDto accountDto) {
		Account? current = await userManager.FindByIdAsync(memberId);
		if (current is null) {
			throw new KeyNotFoundException($"{memberId} not found");
		}

		current.Name = accountDto.Name;
		current.Email = accountDto.Email;
		current.LogoSrc = accountDto.LogoSrc;

		await db.SaveChangesAsync();
		return current;
	}


	[GeneratedRegex("[^a-zA-Z0-9]")]
	private static partial Regex UsernameRegex();
}