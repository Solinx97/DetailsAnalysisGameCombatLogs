using AutoMapper;
using CombatAnalysis.BL.Extensions;
using CombatAnalysis.BL.Mapping;
using CombatAnalysis.CombatParserAPI.Consts;
using CombatAnalysis.CombatParserAPI.Enums;
using CombatAnalysis.CombatParserAPI.Helpers;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Mapping;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var envName = builder.Environment.EnvironmentName;

if (string.Equals(envName, "Development", StringComparison.OrdinalIgnoreCase))
{
    CreateEnvironmentHelper.UseAppsettings(builder.Configuration);
}
else
{
    CreateEnvironmentHelper.UseEnvVariables();
}

var connection = DatabaseProps.Name == nameof(DatabaseType.MSSQL)
    ? DatabaseProps.MSSQLConnectionString
    : DatabaseProps.FirebaseConnectionString;
builder.Services.CombatParserBLDependencies(DatabaseProps.Name, DatabaseProps.DataProcessingType, connection, DBConfiguration.CommandTimeout);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new CombatParserApiMapper());
    mc.AddProfile(new BLMapper());
});

var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddTransient<IHttpClientHelper, HttpClientHelper>();

builder.Services.AddScoped<ICombatDataHelper, CombatDataHelper>();
builder.Services.AddScoped<IPlayerParseInfoHelper, PlayerParseInfoHelper>();

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = DBConfiguration.MaxRequestBodySize;
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
    .MinimumLevel.Warning()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication(); // Enable authentication middleware
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

app.Run();