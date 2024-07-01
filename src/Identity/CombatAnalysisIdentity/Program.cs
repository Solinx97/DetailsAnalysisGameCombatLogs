using AutoMapper;
using CombatAnalysis.Identity.Extensions;
using CombatAnalysis.Identity.Mapping;
using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Core;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterIdentityDependencies(builder.Configuration, "DefaultConnection");

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new IdentityMapper());
});
var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddCors(options =>
{
    options.AddPolicy("default", policy =>
    {
        policy.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddIdentityServer()
            .AddDeveloperSigningCredential() // for Development purposes
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

Encryption.EnctyptionKey = Convert.FromBase64String(builder.Configuration["Authentication:IssuerSigningKey"]);

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
