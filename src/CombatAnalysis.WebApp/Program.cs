using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Helpers;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Middlewares;
using CombatAnalysis.WebApp.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();
builder.Services.AddScoped<RequireAccessTokenAttribute>();

Port.CombatParserApi = builder.Configuration["CombatParserApiPort"] ?? string.Empty;
Port.UserApi = builder.Configuration["UserApiPort"] ?? string.Empty;
Port.ChatApi = builder.Configuration["ChatApiPort"] ?? string.Empty;
Port.CommunicationApi = builder.Configuration["CommunicationApiPort"] ?? string.Empty;
Port.Identity = builder.Configuration["IdentityPort"] ?? string.Empty;

AuthenticationGrantType.Code = builder.Configuration["Authentication:GrantType:Code"] ?? string.Empty;
AuthenticationGrantType.Authorization = builder.Configuration["Authentication:GrantType:Authorization"] ?? string.Empty;
AuthenticationGrantType.RefreshToken = builder.Configuration["Authentication:GrantType:RefreshToken"] ?? string.Empty;

Authentication.ClientId = builder.Configuration["Authentication:ClientId"] ?? string.Empty;
Authentication.ClientScope = builder.Configuration["Authentication:ClientScope"] ?? string.Empty;
Authentication.RedirectUri = builder.Configuration["Authentication:RedirectUri"] ?? string.Empty;
Authentication.IdentityServer = builder.Configuration["Authentication:IdentityServer"] ?? string.Empty;
Authentication.IdentityAuthPath = builder.Configuration["Authentication:IdentityAuthPath"] ?? string.Empty;
Authentication.IdentityRegistryPath = builder.Configuration["Authentication:IdentityRegistryPath"] ?? string.Empty;
Authentication.CodeChallengeMethod = builder.Configuration["Authentication:CodeChallengeMethod"] ?? string.Empty;

if (int.TryParse(builder.Configuration["Authentication:RefreshTokenExpiresDays"], out var refreshTokenExpiresDays))
{
    Authentication.RefreshTokenExpiresDays = refreshTokenExpiresDays;
}

builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024 * 2;
});

builder.Services.AddControllersWithViews();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseRouting();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseMiddleware<AuthTokenMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
