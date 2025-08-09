using Evently.Server.Common.Extensions;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Features.HealthChecks;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class HealthChecksController(IServer server, HealthCheckService healthCheckService) : ControllerBase {
	[HttpGet(Name = "HealthCheck")]
	public async Task<ActionResult> GetHealthcheck() {
		Dictionary<HealthStatus, string> messages = new() {
			{ HealthStatus.Healthy, "Healthy" },
			{ HealthStatus.Unhealthy, "Unhealthy" },
			{ HealthStatus.Degraded, "Degraded" },
		};

		HealthReport healthReport = await healthCheckService.CheckHealthAsync();
		List<ServiceHealth> details = healthReport.Entries
			.Select((pair) => new ServiceHealth(pair.Key, Status: messages[pair.Value.Status]))
			.ToList();

		return Ok(new {
			overallStatus = healthReport.Status == HealthStatus.Healthy ? "Server and DB are healthy" : "Unhealthy",
			serverUrl = server.RootUri()?.AbsoluteUri,
			requestUrl = Request.RootUri().AbsoluteUri,
			details,
		});
	}

	[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Local")]
	private sealed record ServiceHealth(string Service, string Status);
}