using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Common.Domains.Models;

public sealed class Settings {
	public ConnectionStringsSettings ConnectionStrings { get; init; } = new();

	[NotMapped] public AuthSetting Authentication { get; init; } = new();

	[NotMapped] public EmailSettings EmailSettings { get; init; } = new();
}

[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public sealed class ConnectionStringsSettings {
	public string AzureStorageConnectionString { get; init; } = string.Empty;
}

[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public sealed class AuthSetting {
	public OAuthSetting Google { get; init; } = new();
}

[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public sealed class OAuthSetting {
	public string ClientId { get; init; } = string.Empty;
	public string ClientSecret { get; init; } = string.Empty;
}

[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public sealed class EmailSettings {
	public string ActualFrom { get; init; } = string.Empty;
	public string SmtpPassword { get; init; } = string.Empty;
}