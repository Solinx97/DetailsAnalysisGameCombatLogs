using AutoMapper;
using CombatAnalysis.BL.Extensions;
using CombatAnalysis.BL.Mapping;
using CombatAnalysis.CombatParserAPI.Consts;
using CombatAnalysis.CombatParserAPI.Helpers;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Mapping;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dbConfiguration = builder.Configuration.GetSection("DBConfiguration").Get<DBConfiguration>() ?? new DBConfiguration();

builder.Services.CombatParserBLDependencies(builder.Configuration, "DefaultConnection", dbConfiguration.CommandTimeout);

var specs = builder.Configuration.GetSection("Players:Specs").GetChildren();
PlayerInfoConfiguration.Specs = specs?.ToDictionary(entry => entry.Key, entry => entry.Value);

var classes = builder.Configuration.GetSection("Players:Classes").GetChildren();
PlayerInfoConfiguration.Classes = classes?.ToDictionary(entry => entry.Key, entry => entry.Value);

var bosses = builder.Configuration.GetSection("Players:Bosses").GetChildren();
PlayerInfoConfiguration.Bosses = bosses?.ToDictionary(entry => entry.Key, entry => entry.Value);

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
    options.Limits.MaxRequestBodySize = dbConfiguration.MaxRequestBodySize;
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Combat parser API v1");
    options.InjectStylesheet("/swagger-ui/swaggerDark.css");
});

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();