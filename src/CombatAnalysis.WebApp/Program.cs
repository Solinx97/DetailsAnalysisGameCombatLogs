using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Helpers;
using CombatAnalysis.WebApp.Hubs;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Middlewares;
using CombatAnalysis.WebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITokenService, TokenService>();

Port.CombatParserApi = builder.Configuration["CombatParserApiPort"];
Port.UserApi = builder.Configuration["UserApiPort"];
Port.ChatApi = builder.Configuration["ChatApiPort"];
Port.CommunicationApi = builder.Configuration["CommunicationApiPort"];
Port.Identity = builder.Configuration["IdentityPort"];

AuthenticationGrantType.Code = builder.Configuration["Authentication:GrantType:Code"];
AuthenticationGrantType.Authorization = builder.Configuration["Authentication:GrantType:Authorization"];
AuthenticationGrantType.RefreshToken = builder.Configuration["Authentication:GrantType:RefreshToken"];

Authentication.ClientId = builder.Configuration["Authentication:ClientId"];
Authentication.ClientScope = builder.Configuration["Authentication:ClientScope"];
Authentication.RedirectUri = builder.Configuration["Authentication:RedirectUri"];
Authentication.IdentityServer = builder.Configuration["Authentication:IdentityServer"];
Authentication.IdentityAuthPath = builder.Configuration["Authentication:IdentityAuthPath"];
Authentication.IdentityRegistryPath = builder.Configuration["Authentication:IdentityRegistryPath"];
Authentication.CodeChallengeMethod = builder.Configuration["Authentication:CodeChallengeMethod"];
if (int.TryParse(builder.Configuration["Authentication:RefreshTokenExpiresDays"], out var refreshTokenExpiresDays))
{
    Authentication.RefreshTokenExpiresDays = refreshTokenExpiresDays;
}

IHttpClientHelper httpClient = new HttpClientHelper();
builder.Services.AddSingleton(httpClient);
builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024;
});

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseMiddleware<TokenRefreshMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapHub<VoiceChatHub>("/voiceChatHub");

app.MapFallbackToFile("index.html");

app.Run();
