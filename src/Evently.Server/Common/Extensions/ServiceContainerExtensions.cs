using Evently.Server.Common.Adapters.Data;
using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace Evently.Server.Common.Extensions;

public static partial class ServiceContainerExtensions {
	public static IOptions<Settings> LoadAppConfiguration(this IServiceCollection services,
		ConfigurationManager configuration) {
		// load .env variables, in addition to appsettings.json that is loaded by default
		configuration.AddEnvironmentVariables();

		// Inject IOptions<Settings> into the App
		services.Configure<Settings>(configuration);

		// Bind all key value pairs to the Settings Object and return it, as it is used in Program.cs
		Settings settings = new();
		configuration.Bind(settings);

		IOptions<Settings> options = Options.Create(settings);
		return options;
	}

	// Not possible to seed with AppDbContext with OnModelCreating with IdentityUser
	public static async Task SeedDatas(this WebApplication app) {
		using IServiceScope scope = app.Services.CreateScope();
		UserManager<Account> userManager = scope.ServiceProvider.GetRequiredService<UserManager<Account>>();
		AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

		// Define the admin user details
		string email = "host@gmail.com";
		string password = "Host@123";
		// Check if the admin user already exists
		Account? host = await userManager.FindByEmailAsync(email);
		if (host is null) {
			host = new Account {
				UserName = "host",
				Email = email,
				Name = "host",
				PhoneNumber = "1234567",
				EmailConfirmed = true,
			};

			// Create the admin user
			IdentityResult result = await userManager.CreateAsync(host, password);
			if (!result.Succeeded) {
				throw new Exception("Failed to create the host user: " + string.Join(", ", result.Errors));
			}
		}

		email = "guest@gmail.com";
		password = "Guest@123";
		// Check if the admin user already exists
		Account? guest = await userManager.FindByEmailAsync(email);
		if (guest is null) {
			guest = new Account {
				UserName = "guest",
				Email = email,
				Name = "guest",
				PhoneNumber = "7891011",
				EmailConfirmed = true,
			};

			// Create the admin user
			IdentityResult result = await userManager.CreateAsync(guest, password);
			if (!result.Succeeded) {
				throw new Exception("Failed to create the guest user: " + string.Join(", ", result.Errors));
			}
		}

		Category? category1 = await db.Categories.FindAsync((long)1);
		if (category1 is null) {
			category1 = new Category {
				CategoryId = 1,
				CategoryName = "Information Technology",
			};
			await db.Categories.AddAsync(category1);
			await db.SaveChangesAsync();
		}

		Gathering? gathering = await db.Gatherings.FindAsync((long)1);
		if (gathering is null) {
			gathering = new Gathering {
				GatheringId = 1,
				Name = "Party 1",
				Description = "A nice party",
				OrganiserId = host.Id,
			};
			await db.Gatherings.AddAsync(gathering);
			await db.SaveChangesAsync();
		}

		GatheringCategoryDetail? gcDetail = await db.GatheringCategoryDetails
			.Where(detail => detail.CategoryId == category1.CategoryId)
			.Where(detail => detail.GatheringId == gathering.GatheringId)
			.FirstOrDefaultAsync();
		if (gcDetail is null) {
			gcDetail = new GatheringCategoryDetail {
				GatheringId = gathering.GatheringId,
				CategoryId = category1.CategoryId,
			};
			await db.GatheringCategoryDetails.AddAsync(gcDetail);
			await db.SaveChangesAsync();
		}


		Booking? booking = await db.Bookings.FindAsync("abc");
		if (booking is null) {
			booking = new Booking {
				BookingId = "abc",
				GatheringId = gathering.GatheringId,
				AccountId = guest.Id,
			};
			await db.Bookings.AddAsync(booking);
			await db.SaveChangesAsync();
		}
	}

	[GeneratedRegex("postgres://(.*):(.*)@(.*):(.*)/(.*)")]
	private static partial Regex HerokuDbRegex();
}