using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Extensions;
using CombatAnalysis.EnhancedWebApp.Server.Handlers;
using CombatAnalysis.EnhancedWebApp.Server.Helpers;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;
using CombatAnalysis.EnhancedWebApp.Server.Mapping;
using CombatAnalysis.EnhancedWebApp.Server.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// For testing secrets
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();

builder.Services.AddTransient<ExternalApiErrorHandler>();
builder.Services.AddTransient<ApiErrorHandler>();
builder.Services.AddTransient<AuthorizationHandler>();
builder.Services.AddTransient<WoWCharacterGameDataAuthorizationHandler>();
builder.Services.AddTransient<WoWGameDataAuthorizationHandler>();
builder.Services.AddTransient<WoWGameDataAuthAuthorizationHandler>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddApiClients(builder.Configuration);

builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddScoped<IWoWAccountService, WoWAccountService>();
builder.Services.AddScoped<IWoWCharacterService, WoWCharacterService>();
builder.Services.AddScoped<IMythicKeystoneService, MythicKeystoneService>();
builder.Services.AddScoped<IWoWItemService, WoWItemService>();
builder.Services.AddScoped<RequireAccessTokenAttribute>();
builder.Services.AddScoped<RequireRefreshTokenAttribute>();

builder.Services.Configure<Server>(builder.Configuration.GetSection("Server"));
builder.Services.Configure<BattleNet>(builder.Configuration.GetSection("BattleNet"));
builder.Services.Configure<Cluster>(builder.Configuration.GetSection("Cluster"));
builder.Services.Configure<Authentication>(builder.Configuration.GetSection("Authentication"));
builder.Services.Configure<AuthenticationGrantType>(builder.Configuration.GetSection("Authentication:GrantType"));
builder.Services.Configure<AuthenticationClient>(builder.Configuration.GetSection("Authentication:Client"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var loggerFactory = LoggerFactory.Create(builder => { });

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new ProxyApiMapper());
}, loggerFactory);

var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var server = new Server();
builder.Configuration.Bind("Server", server);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddOpenIdConnect(options =>
{
    options.Authority = server.Identity;
    options.ClientId = "web-app";
    options.ResponseType = "code";
    options.SaveTokens = true;
    options.SignedOutCallbackPath = "/";
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
    .WriteTo.File("logs/webapp.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();

builder.Services.AddExceptionHandler<GlobalAPIExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExternalAPIExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.UseExceptionHandler();

app.Run();
