using AutoMapper;
using CombatAnalysis.Identity.Extensions;
using CombatAnalysis.Identity.Mapping;
using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Core;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Mapping;
using CombatAnalysisIdentity.Services;
using Serilog;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

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

var certificate = new X509Certificate2(Path.Combine(Directory.GetCurrentDirectory(), "certs", "solinx.analysis.pfx"), builder.Configuration["Certificates:PWD"]);
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

Authentication.IssuerSigningKey = Convert.FromBase64String(builder.Configuration["Authentication:IssuerSigningKey"]);
Authentication.Issuer = builder.Configuration["Authentication:Issuer"];
Authentication.Protocol = builder.Configuration["Authentication:Protocol"];
if (int.TryParse(builder.Configuration["Authentication:TokenExpiresInMinutes"], out var tokenExpiresInMinutes))
{
    Authentication.TokenExpiresInMinutes = tokenExpiresInMinutes;
}

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
