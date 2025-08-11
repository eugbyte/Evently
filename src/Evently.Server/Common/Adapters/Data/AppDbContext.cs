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
		
		Category category1 = new() {
			CategoryId = 1,
			CategoryName = "Information Technology",
		};
		builder.Entity<Category>().HasData(category1);
	}
}