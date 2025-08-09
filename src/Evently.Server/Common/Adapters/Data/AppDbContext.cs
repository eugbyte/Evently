using Evently.Server.Common.Domains.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Evently.Server.Common.Adapters.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<IdentityUser>(options) {
	public DbSet<Member> Members { get; set; }
	public DbSet<Booking> Bookings { get; set; }
	public DbSet<Gathering> Gatherings { get; set; }
	public DbSet<MemberCategoryDetail> MemberCategoryDetails { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<IdentityUser> IdentityUsers { get; set; }

	protected override void OnModelCreating(ModelBuilder builder) {
		base.OnModelCreating(builder);

		Member member = new() {
			Id = 1,
			Name = "John Doe",
			Email = "john.doe@gmail.com",
			Phone = "088888888",
			Company = "ABC Corporation",
			Role = "Manager",
			Objective = "Networking",
		};
		builder.Entity<Member>().HasData(member);

		Category category1 = new() {
			CategoryId = 1,
			CategoryName = "Information Technology",
		};
		builder.Entity<Category>().HasData(category1);

		Category category2 = new() {
			CategoryId = 2,
			CategoryName = "EdTech",
		};
		builder.Entity<Category>().HasData(category2);

		MemberCategoryDetail memberCategoryDetail = new() {
			MemberId = 1,
			CategoryId = 1,
		};
		builder.Entity<MemberCategoryDetail>().HasData(memberCategoryDetail);

		Gathering gathering = new() {
			GatheringId = 1,
			Name = "The Great Fair",
			Description = "Meet and Greet",
			MemberId = 1,
		};
		builder.Entity<Gathering>().HasData(gathering);

		Booking booking = new() {
			BookingId = "book_v94hv4F9fL",
			MemberId = 1,
			GatheringId = 1,
			RegistrationDateTime = new DateTime(year: 2025, month: 01, day: 01, hour: 1, minute: 0, second: 0, DateTimeKind.Utc),
		};
		builder.Entity<Booking>().HasData(booking);
	}
}