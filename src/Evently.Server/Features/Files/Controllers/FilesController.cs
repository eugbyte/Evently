using Evently.Server.Common.Domains.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel.DataAnnotations;

namespace Evently.Server.Features.Files.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FilesController(ILogger<FilesController> logger, IObjectStorageService objectStorageService) : ControllerBase {
	[HttpGet("object-storage", Name = "GetFile")]
	public async Task<ActionResult> GetFile([Required] [FromQuery] string filePath) {
		logger.LogInformation("filePath: {}", filePath);
		// https://saeventlydevsea.blob.core.windows.net/evently-dev-images/gatherings/20/cover-image.png
		UriBuilder uriBuilder = new(filePath);

		string[] paths = uriBuilder.Path // "/evently-dev-images/gatherings/20/cover-image.png"
			.Split("/") // ["", "evently-dev-images", "gatherings", "20", "cover-image.png"]
			.Skip(1) // skip the first slash from the root url.	
			.ToArray(); // ["evently-dev-images", "gatherings", "20", "cover-image.png"]
		string containerName = paths[0]; // "evently-dev-images"

		filePath = string.Join(separator: '/', values: paths.Skip(1));	// "gatherings/20/cover-image.png"
		try {
			BinaryData binaryData = await objectStorageService.GetFile(containerName, filePath);
			return File(fileContents: binaryData.ToArray(), fileDownloadName: filePath, contentType: binaryData.MediaType ?? GetContentType(filePath));
		} catch (Exception ex) {
			logger.LogError(ex.Message);
		}
		return BadRequest();
	}

	private static string GetContentType(string fileName) {
		FileExtensionContentTypeProvider provider = new();
		if (!provider.TryGetContentType(fileName, contentType: out string? contentType)) {
			contentType = "application/octet-stream"; // Default fallback
		}
		return contentType;
	}
}