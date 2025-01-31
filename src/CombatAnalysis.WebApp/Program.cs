using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Helpers;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Middlewares;
using CombatAnalysis.WebApp.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();
builder.Services.AddScoped<RequireAccessTokenAttribute>();

var envName = builder.Environment.EnvironmentName;

if (string.Equals(envName, "Development", StringComparison.OrdinalIgnoreCase))
{
    CreateEnvironmentHelper.UseAppsettings(builder.Configuration);
}
else
{
    CreateEnvironmentHelper.UseEnvVariables();
}

builder.Services.AddControllersWithViews();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseWebSockets();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<AuthTokenMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
