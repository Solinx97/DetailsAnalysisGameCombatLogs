using AutoMapper;
using CombatAnalysis.CombatParserAPI.Consts;
using CombatAnalysis.CombatParserAPI.Helpers;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Mapping;
using CombatParser.Application.Extensions;
using CombatParser.Application.Mapping;
using CombatParser.Infrastructure.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var databasePropsOptions = new DatabaseProps();
builder.Configuration.Bind("Database", databasePropsOptions);

var databaseConfigsOptions = new DBConfiguration();
builder.Configuration.Bind("DBConfiguration", databaseConfigsOptions);

builder.Services.AddInfrastructure(databasePropsOptions.DefaultConnection);
builder.Services.AddMediatorSource();

var loggerFactory = LoggerFactory.Create(builder => { });

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new CombatParserApiMapper());
    mc.AddProfile(new ApplicationMapper());
}, loggerFactory);

var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddTransient<IHttpClientHelper, HttpClientHelper>();

builder.Services.AddScoped<ISpecializationScoreHelper, SpecializationScoreHelper>();

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = databaseConfigsOptions.MaxRequestBodySize;
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value!.Errors.Count > 0)
            .Select(e => new
            {
                Field = e.Key,
                Errors = e.Value!.Errors.Select(x => x.ErrorMessage)
            });

        return new BadRequestObjectResult(new
        {
            Message = "Validation failed",
            ErrorCount = errors.Count(),
            Errors = errors
        });
    };
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

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var logger = context.HttpContext.RequestServices
            .GetRequiredService<ILogger<Program>>();

        foreach (var kvp in context.ModelState)
        {
            foreach (var error in kvp.Value.Errors)
            {
                logger.LogError(
                    "Validation error. Field: {Field}, Error: {Error}",
                    kvp.Key,
                    error.ErrorMessage);
            }
        }

        return new BadRequestObjectResult(context.ModelState);
    };
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
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