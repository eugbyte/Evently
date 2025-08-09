using Evently.Server.Common.Adapters.Data;
using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Domains.Models;
using Microsoft.EntityFrameworkCore;

namespace Evently.Server.Features.Categories.Services;

public sealed class CategoryService(AppDbContext db) : ICategoryService {
	public async Task<Category> CreateCategory(Category category) {
		await db.Categories.AddAsync(category);
		await db.SaveChangesAsync();
		return category;
	}

	public async Task<PageResult<Category>> GetCategories(long? memberId, bool? approved) {
		IQueryable<Category> query = db.Categories
			.Where((topic) =>
				memberId == null || topic.MemberCategoryDetails.Any((detail) => detail.MemberId == memberId))
			.Where((topic) => approved == null || topic.Approved == approved);

		int totalCount = await query.CountAsync();

		List<Category> topics = await query
			.ToListAsync();

		return new PageResult<Category> {
			Items = topics,
			TotalCount = totalCount,
		};
	}
}