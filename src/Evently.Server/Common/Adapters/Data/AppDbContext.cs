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
}