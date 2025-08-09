using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Common.Domains.Interfaces;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public interface IFileStorageService {
	// mimeType determines the browser's behaviour when the file is accessed directly via the browser,
	// e.g. download ("application/octet-stream") or view ("images/*)" the file.
	Task<Uri> UploadFile(string fileName, BinaryData binaryData, string mimeType = "application/octet-stream");
	Task<Uri> GetFileUri(string fileName);
	Task<BinaryData> GetFile(string fileName);
	Task<bool> IsFileExists(string fileName);
}