﻿using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Models;

namespace Evently.Server.Domains.Interfaces;

public interface ICategoryService {
	Task<PageResult<Category>> GetCategories(long? memberId, bool? approved);
	Task<Category> CreateCategory(Category category);
}