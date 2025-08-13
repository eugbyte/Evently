using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Models;

namespace Evently.Server.Common.Domains.Interfaces;

public interface ICategoryService {
	Task<PageResult<Category>> GetCategories(long? gatheringId, bool? approved);
	Task<Category> CreateCategory(Category category);
}