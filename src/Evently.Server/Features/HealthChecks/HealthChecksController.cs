using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Evently.Server.Features.HealthChecks;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class HealthChecksController(HealthCheckService healthCheckService) : ControllerBase {
	private readonly Dictionary<HealthStatus, string> _statuses = new() {
		{ HealthStatus.Healthy, "Healthy" },
		{ HealthStatus.Unhealthy, "Unhealthy" },
		{ HealthStatus.Degraded, "Degraded" },
	};

	[HttpGet(Name = "HealthCheck")]
	public async Task<ActionResult> GetHealthcheck() {
		HealthReport healthReport = await healthCheckService.CheckHealthAsync();

		List<string> statuses = healthReport.Entries.Select(pair => $"{pair.Key}: {_statuses[pair.Value.Status]}").ToList();
		statuses = ["Server: Healthy", ..statuses];
		return Ok(statuses);
	}
}