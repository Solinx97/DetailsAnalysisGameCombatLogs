using AutoMapper;
using CombatAnalysis.BL.Extensions;
using CombatAnalysis.Identity.Extensions;
using CombatAnalysis.Identity.Mapping;
using CombatAnalysis.Identity.Settings;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Helpers;
using CombatAnalysis.WebApp.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterDependenciesForBL(builder.Configuration, "DefaultConnection");
builder.Services.RegisterIdentityDependencies();

var settings = builder.Configuration.GetSection(nameof(TokenSettings));
var scheme = settings.GetValue<string>(nameof(TokenSettings.AuthScheme));

builder.Services.Configure<TokenSettings>(settings);

Port.CombatParserApi = builder.Configuration.GetValue<string>("CombatParserApiPort");
Port.UserApi = builder.Configuration.GetValue<string>("UserApiPort");

IHttpClientHelper httpClient = new HttpClientHelper();
builder.Services.AddSingleton(httpClient);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new IdentityMappingMapper());
});

var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddControllersWithViews();

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


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
