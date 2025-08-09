using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace Evently.Server.Common.Extensions;

public static class UtilsExtension {
	public static Uri? RootUri(this IServer server) {
		IServerAddressesFeature? addressesFeature = server.Features.Get<IServerAddressesFeature>();
		if (addressesFeature is null) {
			return null;
		}

		List<string> addresses = addressesFeature.Addresses.ToList();
		string address = addresses[0];

		UriBuilder uriBuilder = new(address);
		return uriBuilder.Uri;
	}

	public static Uri RootUri(this HttpRequest request) {
		UriBuilder uriBuilder = new() {
			Scheme = request.Scheme,
			Host = request.Host.Host,
		};
		if (request.Host.Port.HasValue) {
			uriBuilder.Port = request.Host.Port.Value;
		}

		return uriBuilder.Uri;
	}

	public static async Task<BinaryData> ToBinaryData(this IFormFile file) {
		using MemoryStream ms = new();
		await file.CopyToAsync(ms);
		byte[] bytes = ms.ToArray();
		return BinaryData.FromBytes(bytes);
	}
}