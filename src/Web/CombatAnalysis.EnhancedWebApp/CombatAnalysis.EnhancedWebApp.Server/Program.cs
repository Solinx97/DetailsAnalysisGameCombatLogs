using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Helpers;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();
builder.Services.AddScoped<RequireAccessTokenAttribute>();
builder.Services.AddScoped<RequireRefreshTokenAttribute>();

builder.Services.Configure<Cluster>(builder.Configuration.GetSection("Cluster"));
builder.Services.Configure<Server>(builder.Configuration.GetSection("Server"));
builder.Services.Configure<Authentication>(builder.Configuration.GetSection("Authentication"));
builder.Services.Configure<AuthenticationGrantType>(builder.Configuration.GetSection("Authentication:GrantType"));
builder.Services.Configure<AuthenticationClient>(builder.Configuration.GetSection("Authentication:Client"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
