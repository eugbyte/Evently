using Evently.Server.Common.Adapters.Data;
using Evently.Server.Common.Domains.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Evently.Server.Common.Extensions;

public static class SeederExtension {

	// Not possible to seed with AppDbContext with OnModelCreating with IdentityUser
	public static async Task SeedDatas(this WebApplication app) {
		using IServiceScope scope = app.Services.CreateScope();
		UserManager<Account> userManager = scope.ServiceProvider.GetRequiredService<UserManager<Account>>();
		AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

		// Define the admin user details
		string email = "host@gmail.com";
		string password = "Host@123";
		// Check if the admin user already exists
		Account? account1 = await userManager.FindByEmailAsync(email);
		if (account1 is null) {
			account1 = new Account {
				UserName = "host",
				Email = email,
				Name = "host",
				PhoneNumber = "1234567",
				EmailConfirmed = true,
			};

			// Create the admin user
			IdentityResult result = await userManager.CreateAsync(account1, password);
			if (!result.Succeeded) {
				throw new Exception("Failed to create the host user: " + string.Join(", ", result.Errors));
			}
		}

		email = "guest@gmail.com";
		password = "Guest@123";
		// Check if the admin user already exists
		Account? account2 = await userManager.FindByEmailAsync(email);
		if (account2 is null) {
			account2 = new Account {
				UserName = "guest",
				Email = email,
				Name = "guest",
				PhoneNumber = "7891011",
				EmailConfirmed = true,
			};

			// Create the admin user
			IdentityResult result = await userManager.CreateAsync(account2, password);
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

		// Add more categories for variety
		Category? category2 = await db.Categories.FindAsync((long)2);
		if (category2 is null) {
			category2 = new Category {
				CategoryId = 2,
				CategoryName = "Business & Networking",
			};
			await db.Categories.AddAsync(category2);
			await db.SaveChangesAsync();
		}

		Category? category3 = await db.Categories.FindAsync((long)3);
		if (category3 is null) {
			category3 = new Category {
				CategoryId = 3,
				CategoryName = "Arts & Culture",
			};
			await db.Categories.AddAsync(category3);
			await db.SaveChangesAsync();
		}

		// Singapore timezone offset (+8 hours from UTC)
		TimeSpan singaporeOffset = TimeSpan.Zero;

		// Gatherings organized by account1 (host) and account2 (guest)
		Gathering[] gatherings = [
			new() {
				GatheringId = 1,
				Name = "Tech Innovation Summit",
				Description = "A comprehensive summit exploring the latest in AI and machine learning",
				Location = "Marina Bay Sands Convention Centre, Singapore",
				Start = new DateTimeOffset(year: 2025, month: 12, day: 5, hour: 9, minute: 0, second: 0, singaporeOffset),
				End = new DateTimeOffset(year: 2025, month: 12, day: 5, hour: 17, minute: 0, second: 0, singaporeOffset),
				OrganiserId = account1.Id,
			},
			new() {
				GatheringId = 2,
				Name = "Startup Networking Night",
				Description = "Connect with fellow entrepreneurs and investors",
				Location = "Clarke Quay Central, Singapore",
				Start = new DateTimeOffset(year: 2025, month: 12, day: 10, hour: 18, minute: 30, second: 0, singaporeOffset),
				End = new DateTimeOffset(year: 2025, month: 12, day: 10, hour: 22, minute: 0, second: 0, singaporeOffset),
				OrganiserId = account1.Id,
			},
			new() {
				GatheringId = 3,
				Name = "Digital Art Exhibition",
				Description = "Showcasing contemporary digital art from emerging artists",
				Location = "National Gallery Singapore",
				Start = new DateTimeOffset(year: 2025, month: 12, day: 15, hour: 10, minute: 0, second: 0, singaporeOffset),
				End = new DateTimeOffset(year: 2025, month: 12, day: 15, hour: 18, minute: 0, second: 0, singaporeOffset),
				OrganiserId = account1.Id,
			},
			new() {
				GatheringId = 4,
				Name = "Web Development Workshop",
				Description = "Learn modern web development techniques and best practices",
				Location = "Singapore Science Centre",
				Start = new DateTimeOffset(year: 2025, month: 12, day: 8, hour: 13, minute: 0, second: 0, singaporeOffset),
				End = new DateTimeOffset(year: 2025, month: 12, day: 8, hour: 17, minute: 0, second: 0, singaporeOffset),
				OrganiserId = account2.Id,
			},
			new() {
				GatheringId = 5,
				Name = "Business Strategy Seminar",
				Description = "Advanced strategies for scaling your business",
				Location = "Raffles City Convention Centre, Singapore",
				Start = new DateTimeOffset(year: 2025, month: 12, day: 20, hour: 14, minute: 0, second: 0, singaporeOffset),
				End = new DateTimeOffset(year: 2025, month: 12, day: 20, hour: 16, minute: 30, second: 0, singaporeOffset),
				OrganiserId = account2.Id,
			},
			new() {
				GatheringId = 6,
				Name = "Photography Masterclass",
				Description = "Professional photography techniques and portfolio building",
				Location = "Gardens by the Bay, Singapore",
				Start = new DateTimeOffset(year: 2025, month: 12, day: 22, hour: 8, minute: 0, second: 0, singaporeOffset),
				End = new DateTimeOffset(year: 2025, month: 12, day: 22, hour: 12, minute: 0, second: 0, singaporeOffset),
				OrganiserId = account2.Id,
			},
		];

		// Category details for gatherings
		GatheringCategoryDetail[] gatheringCategoryDetails = [
			new() {
				GatheringId = 1,
				CategoryId = category1.CategoryId, // Tech Innovation Summit -> IT
			},
			new() {
				GatheringId = 2,
				CategoryId = category2.CategoryId, // Startup Networking Night -> Business
			},
			new() {
				GatheringId = 3,
				CategoryId = category3.CategoryId, // Digital Art Exhibition -> Arts
			},
			new() {
				GatheringId = 4,
				CategoryId = category1.CategoryId, // Web Development Workshop -> IT
			},
			new() {
				GatheringId = 5,
				CategoryId = category2.CategoryId, // Business Strategy Seminar -> Business
			},
			new() {
				GatheringId = 6,
				CategoryId = category3.CategoryId, // Photography Masterclass -> Arts
			},
		];

		foreach (Gathering gathering in gatherings) {
			Gathering? existingGathering = await db.Gatherings.FindAsync(gathering.GatheringId);
			if (existingGathering is null) {
				await db.Gatherings.AddAsync(gathering);
				await db.SaveChangesAsync();
			}
		}

		foreach (GatheringCategoryDetail gatheringCategoryDetail in gatheringCategoryDetails) {
			GatheringCategoryDetail? existingGcDetail = await db.GatheringCategoryDetails
				.Where(detail => detail.CategoryId == gatheringCategoryDetail.CategoryId)
				.Where(detail => detail.GatheringId == gatheringCategoryDetail.GatheringId)
				.FirstOrDefaultAsync();
			if (existingGcDetail is null) {
				await db.GatheringCategoryDetails.AddAsync(gatheringCategoryDetail);
				await db.SaveChangesAsync();
			}
		}

		// Add sample bookings
		Booking[] bookings = [
			new() {
				BookingId = "abc",
				GatheringId = 1,
				AccountId = account2.Id,
			},
			new() {
				BookingId = "def",
				GatheringId = 2,
				AccountId = account2.Id,
			},
			new() {
				BookingId = "ghi",
				GatheringId = 4,
				AccountId = account1.Id,
			},
			new() {
				BookingId = "jkl",
				GatheringId = 5,
				AccountId = account1.Id,
			},
		];

		foreach (Booking booking in bookings) {
			Booking? existingBooking = await db.Bookings.FindAsync(booking.BookingId);
			if (existingBooking is null) {
				await db.Bookings.AddAsync(booking);
				await db.SaveChangesAsync();
			}
		}
	}
}