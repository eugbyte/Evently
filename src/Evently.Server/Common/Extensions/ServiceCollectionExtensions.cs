using Evently.Server.Domains.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace Evently.Server.Common.Extensions;

public static partial class ServiceCollectionExtensions {
	public static IOptions<Settings> LoadAppConfiguration(this IServiceCollection services,
		ConfigurationManager configuration) {
		// load .env variables, in addition to appsettings.json that is loaded by default
		configuration.AddEnvironmentVariables();

		// Inject IOptions<Settings> into the App
		services.Configure<Settings>(configuration);

		// Bind all key value pairs to the Settings Object and return it, as it is used in Program.cs
		Settings settings = new();
		configuration.Bind(settings);

		IOptions<Settings> options = Options.Create(settings);
		return options;
	}

	[GeneratedRegex("postgres://(.*):(.*)@(.*):(.*)/(.*)")]
	private static partial Regex HerokuDbRegex();
}