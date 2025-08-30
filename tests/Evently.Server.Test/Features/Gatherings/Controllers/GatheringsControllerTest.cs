using Evently.Server.Common.Adapters.Data;
using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Domains.Models;
using Evently.Server.Features.Gatherings.Controllers;
using Evently.Server.Features.Gatherings.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Evently.Server.Test.Features.Gatherings.Controllers;

public class GatheringsControllerTests : IDisposable {
	private readonly SqliteConnection _conn;
	private readonly AppDbContext _dbContext;
	private readonly GatheringsController _gatheringsController;

	public GatheringsControllerTests() {
		_conn = new SqliteConnection("Filename=:memory:");
		_conn.Open();

		// These options will be used by the context instances in this test suite, including the connection opened above.
		DbContextOptions<AppDbContext> contextOptions = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlite(_conn)
			.Options;

		// Create the schema and seed some data
		AppDbContext dbContext = new(contextOptions);

		dbContext.Database.EnsureCreated();
		_dbContext = dbContext;

		IGatheringService gatheringService = new GatheringService(_dbContext, validator: new GatheringValidator());
		Mock<IFileStorageService> mockStorageService = new();
		_gatheringsController = new GatheringsController(gatheringService, mockStorageService.Object);
	}

	public void Dispose() {
		_dbContext.Dispose();
		_conn.Dispose();
	}

	[Fact]
	public async Task GetGathering_WithInvalidId_ReturnsNotFound() {
		// Arrange
		const long invalidId = -1;
		// Act
		ActionResult<Gathering> result = await _gatheringsController.GetGathering(invalidId);
		// Assert
		Assert.IsType<NotFoundResult>(result.Result);
	}

	[Fact]
	public async Task CreateGathering_Return_Forbid() {
		// Arrange
		GatheringReqDto createRequest = new(
			GatheringId: 0,
			"New Test Gathering",
			"Test Description",
			Start: DateTimeOffset.UtcNow.AddDays(1),
			End: DateTimeOffset.UtcNow.AddDays(1).AddHours(2),
			CancellationDateTime: null,
			"Test Location",
			"empty-user-12345",
			CoverSrc: null,
			GatheringCategoryDetails: []
		);

		// Act
		ActionResult<Gathering> result = await _gatheringsController.CreateGathering(createRequest, coverImg: null);

		// Assert
		Assert.IsType<UnauthorizedResult>(result.Result);
	}
}