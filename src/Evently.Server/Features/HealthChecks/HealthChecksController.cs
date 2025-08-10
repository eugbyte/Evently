using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Evently.Server.Features.HealthChecks;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class HealthChecksController(HealthCheckService healthCheckService) : ControllerBase {
	[HttpGet(Name = "HealthCheck")]
	public async Task<ActionResult> GetHealthcheck() {
		HealthReport healthReport = await healthCheckService.CheckHealthAsync();
		return Ok(healthReport);
	}
}