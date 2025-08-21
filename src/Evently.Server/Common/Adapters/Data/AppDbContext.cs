using Evently.Server.Common.Domains.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Evently.Server.Common.Adapters.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<Account>(options) {
	public DbSet<Account> Accounts { get; set; }
	public DbSet<Booking> Bookings { get; set; }
	public DbSet<Gathering> Gatherings { get; set; }
	public DbSet<GatheringCategoryDetail> GatheringCategoryDetails { get; set; }
	public DbSet<Category> Categories { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);
		
		// Postgres has a bug where if you seed the data (as is done in SeederExtension) with specified ID, starting from 1,
		// the PgSql driver does not auto increment and re-use those ids.
		// https://stackoverflow.com/a/77933866/6514532
		modelBuilder.Entity<Gathering>().Property(g => g.GatheringId)
			.HasIdentityOptions(startValue: 20);
		
		modelBuilder.Entity<Category>().Property(c => c.CategoryId)
			.HasIdentityOptions(startValue: 20);
		
		modelBuilder.Entity<Account>().HasData(new Account
		{
			Id = "empty-user-12345", // Fixed constant ID
			UserName = "empty_user",
			NormalizedUserName = "EMPTY_USER",
			Email = "empty@example.com",
			NormalizedEmail = "EMPTY@EXAMPLE.COM",
			Name = "Empty User",
			EmailConfirmed = false,
			PasswordHash = null, // No password - unusable account
			SecurityStamp = "", // Fixed constant
			ConcurrencyStamp = "", // Fixed constant
			PhoneNumber = null,
			PhoneNumberConfirmed = false,
			TwoFactorEnabled = false,
			LockoutEnabled = true,
			AccessFailedCount = 0
		});

	}
}