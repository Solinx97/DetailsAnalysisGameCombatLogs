using AutoMapper;
using CombatAnalysis.Identity.Extensions;
using CombatAnalysis.Identity.Mapping;
using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Core;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Mapping;
using CombatAnalysisIdentity.Services;
using Serilog;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

Port.UserApi = builder.Configuration["UserApiPort"];
Port.Identity = builder.Configuration["IdentityPort"];
Port.Identity = builder.Configuration["IdentityPort"];

AuthenticationGrantType.Code = builder.Configuration["Authentication:GrantType:Code"];
AuthenticationGrantType.Authorization = builder.Configuration["Authentication:GrantType:Authorization"];
AuthenticationGrantType.RefreshToken = builder.Configuration["Authentication:GrantType:RefreshToken"];

Authentication.IssuerSigningKey = Convert.FromBase64String(builder.Configuration["Authentication:IssuerSigningKey"]);
Authentication.Issuer = builder.Configuration["Authentication:Issuer"];
Authentication.Protocol = builder.Configuration["Authentication:Protocol"];
if (int.TryParse(builder.Configuration["Authentication:AccessTokenExpiresMins"], out var accessTokenExpiresMins))
{
    Authentication.AccessTokenExpiresMins = accessTokenExpiresMins;
}
if (int.TryParse(builder.Configuration["Authentication:RefreshTokenExpiresDays"], out var refreshTokenExpiresDays))
{
    Authentication.RefreshTokenExpiresDays = refreshTokenExpiresDays;
}

builder.Services.RegisterIdentityDependencies(builder.Configuration, "DefaultConnection");

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new IdentityMapper());
    mc.AddProfile(new CombatAnalysisIdentityMapper());
});
var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddTransient<IUserAuthorizationService, UserAuthorizationService>();

// Add services to the container.
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();
builder.Services.AddCors(options =>
{
    options.AddPolicy("default", policy =>
    {
        policy.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var certificate = new X509Certificate2(Path.Combine(Directory.GetCurrentDirectory(), "certs", builder.Configuration["Certificates:Name"]), builder.Configuration["Certificates:PWD"]);
builder.Services.AddIdentityServer()
            .AddSigningCredential(certificate)
            .AddInMemoryApiResources(Config.GetApiResources())
            .AddInMemoryIdentityResources(Config.GetIdentityResources())
            .AddInMemoryClients(Config.GetClients())
            .AddInMemoryApiScopes(Config.ApiScopes);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("default");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.MapRazorPages();

app.MapControllers();

app.Run();
