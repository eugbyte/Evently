using Evently.Server.Common.Domains.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Evently.Server.Common.Adapters.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<IdentityUser>(options) {
	public DbSet<Member> Members { get; set; }
	public DbSet<Booking> Bookings { get; set; }
	public DbSet<Gathering> Gatherings { get; set; }
	public DbSet<GatheringCategoryDetail> GatheringCategoryDetails { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<IdentityUser> IdentityUsers { get; set; }

	protected override void OnModelCreating(ModelBuilder builder) {
		base.OnModelCreating(builder);

		Category category1 = new() {
			CategoryId = 1,
			CategoryName = "Information Technology",
		};
		builder.Entity<Category>().HasData(category1);

		Member member = new() {
			MemberId = 1,
			Name = "John Doe",
			Email = "john.doe@gmail.com",
		};
		builder.Entity<Member>().HasData(member);

		Gathering gathering = new() {
			GatheringId = 1,
			Name = "The Great Fair",
			Description = "Meet and Greet",
			OrganiserId = 1,
			Start = new DateTimeOffset(year: 2025, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.Zero),
			End = new DateTimeOffset(year: 2025, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.Zero),
		};
		builder.Entity<Gathering>().HasData(gathering);

		GatheringCategoryDetail gatheringCategoryDetail = new() {
			GatheringId = 1,
			CategoryId = 1,
		};
		builder.Entity<GatheringCategoryDetail>().HasData(gatheringCategoryDetail);
	}
}