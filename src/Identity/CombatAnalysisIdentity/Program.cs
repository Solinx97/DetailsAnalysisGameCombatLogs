using AutoMapper;
using CombatAnalysis.Identity.Extensions;
using CombatAnalysis.Identity.Mapping;
using CombatAnalysis.Identity.Security;
using CombatAnalysis.UserBL.Extensions;
using CombatAnalysis.UserBL.Mapping;
using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Core;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Mapping;
using CombatAnalysisIdentity.Services;
using Duende.IdentityServer.Configuration;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Cluster>(builder.Configuration.GetSection("API"));
builder.Services.Configure<Authentication>(builder.Configuration.GetSection("Authentication"));
builder.Services.Configure<AuthenticationClient>(builder.Configuration.GetSection("Authentication:Client"));
builder.Services.Configure<AuthenticationGrantType>(builder.Configuration.GetSection("Authentication:GrantType"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));

var databasePropsOptions = new DatabaseProps();
builder.Configuration.Bind("Database", databasePropsOptions);

builder.Services.RegisterIdentityDependencies(databasePropsOptions.AppIdentity);
builder.Services.UserBLDependencies(databasePropsOptions.UserConnection);

var loggerFactory = LoggerFactory.Create(builder => { });

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new IdentityMapper());
    mc.AddProfile(new UserBLMapper());
    mc.AddProfile(new CombatAnalysisIdentityMapper());
}, loggerFactory);
var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddTransient<IUserAuthorizationService, UserAuthorizationService>();
builder.Services.AddTransient<IEmailService, EmailService>();

//var redisOptions = new Redis();
//builder.Configuration.Bind("Redis", redisOptions);
//builder.Services.AddSingleton<IConnectionMultiplexer>(
//    ConnectionMultiplexer.Connect(redisOptions.Server)
//);

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

builder.Services.AddAuthentication("Cookies")
       .AddCookie("Cookies");

var certificateOptions = new Certificate();
builder.Configuration.Bind("Certificate", certificateOptions);

var certificate = new X509Certificate2(certificateOptions.PfxPath, certificateOptions.PWD);
builder.Services.AddIdentityServer(options =>
            {
                options.Authentication.CookieAuthenticationScheme = "Cookies";
            })
            .AddSigningCredential(certificate)
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(databasePropsOptions.Identity,
                        sql => sql.MigrationsAssembly(typeof(Program).Assembly.FullName));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(databasePropsOptions.Identity,
                        sql => sql.MigrationsAssembly(typeof(Program).Assembly.FullName));
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 3600;
            });

builder.Services.Configure<IdentityServerOptions>(options =>
{
    options.UserInteraction.LoginUrl = "/Account/Login";
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
    .WriteTo.File("logs/identity.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();
app.InitializeIdentity();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseCors("default");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.MapRazorPages();

app.MapControllers();

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
