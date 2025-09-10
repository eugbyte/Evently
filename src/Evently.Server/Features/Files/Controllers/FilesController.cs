using Evently.Server.Common.Domains.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel.DataAnnotations;

namespace Evently.Server.Features.Files.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FilesController(ILogger<FilesController> logger, IFileStorageService fileStorageService) : ControllerBase {
	[HttpGet("object-storage", Name = "GetFile")]
	public async Task<ActionResult> GetFile([Required][FromQuery] string filePath) {
		logger.LogInformation("filePath: {}", filePath);
		// https://saeventlydevsea.blob.core.windows.net/evently-dev-images/gatherings/20/cover-image.png
		UriBuilder uriBuilder = new(filePath);

		List<string> paths = uriBuilder.Path.Split("/").Skip(1).ToList();	// skip the first slash from the root url.
		string containerName = paths[0];
		
		filePath = string.Join('/', paths.Skip(1));
		try {
			BinaryData binaryData = await fileStorageService.GetFile(containerName, filePath);
			return File(binaryData.ToArray(), binaryData.MediaType ?? GetContentType(filePath), filePath);
		} catch (Exception ex) {
			logger.LogError(ex.Message);
		}
		return this.BadRequest();
	}
	
	private static string GetContentType(string fileName) {
		var provider = new FileExtensionContentTypeProvider();
		if (!provider.TryGetContentType(fileName, out string? contentType)) {
			contentType = "application/octet-stream"; // Default fallback
		}
		return contentType;
	}

}