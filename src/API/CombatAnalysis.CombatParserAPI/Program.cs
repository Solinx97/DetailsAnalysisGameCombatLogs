using AutoMapper;
using CombatAnalysis.BL.Extensions;
using CombatAnalysis.BL.Mapping;
using CombatAnalysis.CombatParserAPI.Consts;
using CombatAnalysis.CombatParserAPI.Helpers;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Mapping;
using CombatParser.Infrastructure.Extensions;
using CombatParser.Application.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using CombatParser.Application.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Players>(builder.Configuration.GetSection("Players"));

var databasePropsOptions = new DatabaseProps();
builder.Configuration.Bind("Database", databasePropsOptions);

var databaseConfigsOptions = new DBConfiguration();
builder.Configuration.Bind("DBConfiguration", databaseConfigsOptions);

builder.Services.CombatParserBLDependencies(databasePropsOptions.DefaultConnection, databaseConfigsOptions.CommandTimeout);
builder.Services.AddInfrastructure(databasePropsOptions.DefaultConnection);
builder.Services.AddMediatorSource();

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new CombatParserApiMapper());
    mc.AddProfile(new BLMapper());
    mc.AddProfile(new ApplicationMapper());
});

var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddTransient<IHttpClientHelper, HttpClientHelper>();

builder.Services.AddScoped<ISpecializationScoreHelper, SpecializationScoreHelper>();

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = databaseConfigsOptions.MaxRequestBodySize;
});
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Combat parser API",
        Version = "v1",
    });
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
    .WriteTo.File("logs/parserapi.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Combat parser API v1");
    options.InjectStylesheet("/swagger-ui/swaggerDark.css");
});

app.UseStaticFiles();
app.UseHttpsRedirection();

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