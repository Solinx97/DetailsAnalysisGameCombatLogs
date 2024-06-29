using AutoMapper;
using CombatAnalysis.CustomerBL.Extensions;
using CombatAnalysis.CustomerBL.Mapping;
using CombatAnalysis.Identity.Extensions;
using CombatAnalysisIdentity.Core;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.CustomerBLDependencies(builder.Configuration, "DefaultConnection");
builder.Services.RegisterIdentityDependencies();

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new CustomerBLMapper());
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
