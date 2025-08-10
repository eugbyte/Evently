using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Domains.Models;
using Microsoft.Extensions.Options;

namespace Evently.Server.Features.Files.Services;

// Based on https://tinyurl.com/5pam66xn
public sealed class FileService(IOptions<Settings> settings) : IFileStorageService {
	private const string ContainerName = "evently-images";

	private readonly BlobServiceClient _blobServiceClient =
		new(settings.Value.ConnectionStrings.AzureStorageConnectionString);

	public async Task<Uri> UploadFile(string fileName, BinaryData binaryData,
		string mimeType = "application/octet-stream") {
		BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
		await containerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

		BlobClient blobClient = containerClient.GetBlobClient(fileName);

		BlobUploadOptions uploadOptions = new() {
			HttpHeaders = new BlobHttpHeaders {
				ContentType = mimeType,
			},
			// https://tinyurl.com/ms4hvsta
			// By default, there will be condition to prevent overwrite.
			// Set the conditions to null so that overwrite will be allowed.
			Conditions = null,
		};
		await blobClient.UploadAsync(binaryData, uploadOptions);
		return blobClient.Uri;
	}

	public async Task<bool> IsFileExists(string fileName) {
		BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
		BlobClient blobClient = containerClient.GetBlobClient(fileName);
		Response<bool> result = await blobClient.ExistsAsync();
		return result.Value;
	}

	public Task<Uri> GetFileUri(string fileName) {
		BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
		BlobClient blobClient = containerClient.GetBlobClient(fileName);
		return Task.FromResult(blobClient.Uri);
	}

	public async Task<BinaryData> GetFile(string fileName) {
		BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
		BlobClient blobClient = containerClient.GetBlobClient(fileName);

		using MemoryStream ms = new();
		await blobClient.DownloadToAsync(ms);

		BinaryData data = await BinaryData.FromStreamAsync(ms);
		return data;
	}
}