using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Helpers;
using CombatAnalysis.WebApp.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

Port.CombatParserApi = builder.Configuration.GetValue<string>("CombatParserApiPort");
Port.UserApi = builder.Configuration.GetValue<string>("UserApiPort");
Port.ChatApi = builder.Configuration.GetValue<string>("ChatApiPort");
Port.CommunicationApi = builder.Configuration.GetValue<string>("CommunicationApiPort");
Port.Identity = builder.Configuration.GetValue<string>("Identity");

var settings = builder.Configuration.GetSection(nameof(Authorization));

Authorization.ClientId = settings.GetValue<string>("ClientId");
Authorization.RedirectUri = settings.GetValue<string>("RedirectUri");

IHttpClientHelper httpClient = new HttpClientHelper();
builder.Services.AddSingleton(httpClient);

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie();

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
