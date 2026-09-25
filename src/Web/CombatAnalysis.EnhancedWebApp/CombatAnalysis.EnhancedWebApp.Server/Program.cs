using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Handlers;
using CombatAnalysis.EnhancedWebApp.Server.Helpers;
using CombatAnalysis.EnhancedWebApp.Server.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;
using CombatAnalysis.EnhancedWebApp.Server.Mapping;
using CombatAnalysis.EnhancedWebApp.Server.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();

builder.Services.AddTransient<ExternalApiErrorHandler>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<WoWCharacterGameDataAuthorizationHandler>();
builder.Services.AddHttpClient<IWoWUserGameDataApiClient, WoWUserGameDataApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("BattleNet:BattleNetAPI").Value ?? "");
})
    .AddHttpMessageHandler<WoWCharacterGameDataAuthorizationHandler>()
    .AddHttpMessageHandler<ExternalApiErrorHandler>();


builder.Services.AddTransient<WoWGameDataAuthorizationHandler>();
builder.Services.AddHttpClient<IWoWGameDataApiClient, WoWGameDataApiClient>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration.GetSection("BattleNet:BattleNetAPI").Value ?? "");
    })
    .AddHttpMessageHandler<WoWGameDataAuthorizationHandler>()
    .AddHttpMessageHandler<ExternalApiErrorHandler>();

builder.Services.AddHttpClient<IWoWCharacterGameDataApiClient, WoWCharacterGameDataApiClient>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration.GetSection("BattleNet:BattleNetAPI").Value ?? "");
    })
    .AddHttpMessageHandler<WoWGameDataAuthorizationHandler>()
    .AddHttpMessageHandler<ExternalApiErrorHandler>();

builder.Services.AddTransient<WoWGameDataAuthAuthorizationHandler>();
builder.Services.AddHttpClient<IWoWGameDataAuthApiClient, WoWGameDataAuthApiClient>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration.GetSection("BattleNet:BattleNetAutAPI").Value ?? "");
    })
    .AddHttpMessageHandler<WoWGameDataAuthAuthorizationHandler>()
    .AddHttpMessageHandler<ExternalApiErrorHandler>();

builder.Services.AddScoped<IAchievementService, AchievementService>();
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

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var ex = exceptionHandlerPathFeature?.Error;

        Log.Error(ex, "Unhandled exception occurred");

        var result = new
        {
            message = "An unexpected error occurred. Please try again later."
        };

        await context.Response.WriteAsJsonAsync(result);
    });
});

app.Run();
