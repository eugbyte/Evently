using Evently.Server.Common.Adapters.Blazor;
using Evently.Server.Common.Adapters.Data;
using Evently.Server.Common.Domains.Entities;
using Evently.Server.Common.Domains.Interfaces;
using Evently.Server.Common.Domains.Models;
using Evently.Server.Common.Extensions;
using Evently.Server.Features.Auths.Services;
using Evently.Server.Features.Bookings.Services;
using Evently.Server.Features.Categories.Services;
using Evently.Server.Features.Emails.Services;
using Evently.Server.Features.FileStorage.Services;
using Evently.Server.Features.Gatherings.Services;
using Evently.Server.Features.Members.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using AccountsService=Evently.Server.Features.Auths.Services.AccountsService;
using BlazorHtmlRenderer=Microsoft.AspNetCore.Components.Web.HtmlRenderer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

bool isHeroku = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DYNO"));

ILoggerFactory logFactory = LoggerFactory.Create((logBuilder) => {
	logBuilder.AddSimpleConsole((opts) => opts.ColorBehavior = LoggerColorBehavior.Disabled);
});
ILogger<Program> logger = logFactory.CreateLogger<Program>();

// Inject appsettings.json into the application
IOptions<Settings> settings = builder.Services.LoadAppConfiguration(config);

// register DB
// retrieve the heroku postgres db conn string, otherwise, get the local default
string? dbConnStr = builder.Configuration.GetConnectionString("WebApiDatabase");
logger.LogValue("dbConnStr", dbConnStr);
builder.Services.AddDbContext<AppDbContext>((options) => {
	options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
	options.UseNpgsql(dbConnStr, npgsqlOptionsAction: pgOpt => pgOpt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
});

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions((options) =>
	options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddTransient<IMemberService, MemberService>();
builder.Services.AddTransient<IGatheringService, GatheringService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IAccountsService, AccountsService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthorizationHandler, SameUserAuthorizationHandler>();
builder.Services.AddHealthChecks()
	.AddDbContextCheck<AppDbContext>();
builder.Services.AddSingleton<IFileStorageService, ImageStorageService>();
builder.Services.AddTransient<IBookingService, BookingService>();

// MediaRenderer relies on BlazorHtmlRenderer
builder.Services.AddScoped<BlazorHtmlRenderer>();
builder.Services.AddScoped<IMediaRenderer, MediaRenderer>();

// singleton background service have indirect dependencies on the channel, emailService and mediaRenderer
Channel<EmailMqPayload> channel = Channel.CreateBounded<EmailMqPayload>(100); // Set capacity to 100
builder.Services.AddSingleton(channel.Reader);
builder.Services.AddSingleton(channel.Writer);
builder.Services.AddSingleton<IEmailer, Emailer>();
builder.Services.AddHostedService<EmailBackgroundService>();


// Fluent validation dependency injection without automatic registration
builder.Services.AddScoped<IValidator<Member>, MemberValidator>();
builder.Services.AddScoped<IValidator<Gathering>, GatheringValidator>();

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
	.AddEntityFrameworkStores<AppDbContext>();

// https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/7.0/default-authentication-scheme#new-behavior
// No default auth scheme is set
builder.Services.AddAuthentication()
	.AddCookie()
	.AddGoogle((options) => {
		options.ClientId = settings.Value.Authentication.Google.ClientId;
		options.ClientSecret = settings.Value.Authentication.Google.ClientSecret;
		options.CallbackPath = "/signin-google"; // rmb to resister in the Google oauth dashboard
		options.SignInScheme =
			IdentityConstants
				.ExternalScheme; // important to default to external scheme - https://stackoverflow.com/a/78674926/6514532
		options.SaveTokens = true;

		// For debugging purpose
		options.Events.OnRedirectToAuthorizationEndpoint = (context) => {
			// logger.LogInformation("Request Path: {Request}", context.Request.FullUri().AbsoluteUri);
			context.HttpContext.Response.Redirect(context.RedirectUri);
			return Task.CompletedTask;
		};
	});
builder.Services.AddAuthorizationBuilder()
	.AddPolicy(SameUserRequirement.PolicyName,
		configurePolicy: (policy) =>
			policy.Requirements.Add(new SameUserRequirement()));

// Add razor pages support to render Blazor files
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

WebApplication app = builder.Build();

// Needed for heroku
// 1. Forward headers through heroku proxy (https://devcenter.heroku.com/articles/aspnetcore-app-configuration#enforcing-https)
app.UseForwardedHeaders();
// 2. Migration heroku postgres db (https://stackoverflow.com/a/76597872/6514532)
using (IServiceScope serviceScope = app.Services.CreateScope()) {
	AppDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
	await dbContext.Database.MigrateAsync();
}

// To serve the Svelte SPA files
app.UseFileServer();

// needed for Blazor
app.UseAntiforgery();
app.MapRazorComponents<BlazorApp>()
	.AddInteractiveServerRenderMode();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// To serve the Svelte SPA files
app.MapFallbackToFile("/index.html");

app.Run();

// Needed for unit testing
// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program;