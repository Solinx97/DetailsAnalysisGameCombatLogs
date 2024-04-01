using AutoMapper;
using CombatAnalysis.CommunicationAPI.Mapping;
using CombatAnalysis.CommunicationAPI.Middleware;
using CombatAnalysis.CommunicationBL.Extensions;
using CombatAnalysis.CommunicationBL.Mapping;
using CombatAnalysis.CustomerBL.Extensions;
using CombatAnalysis.Identity.Extensions;
using CombatAnalysis.Identity.Settings;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.CommunicationBLDependencies(builder.Configuration, "DefaultConnection");
builder.Services.CustomerBLDependencies(builder.Configuration, "UserConnection");
builder.Services.RegisterIdentityDependencies();

var settings = builder.Configuration.GetSection(nameof(TokenSettings));
var scheme = settings.GetValue<string>(nameof(TokenSettings.AuthScheme));

builder.Services.Configure<TokenSettings>(settings);

var loggerFactory = new LoggerFactory();
var logger = new Logger<ILogger>(loggerFactory);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new CommunicationMapper());
    mc.AddProfile(new BLMapper());
});

var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddSingleton<ILogger>(logger);
builder.Services.AddAuthentication((options) =>
{
    options.DefaultChallengeScheme = "Basic";
    options.AddScheme("Basic", (builder) =>
    {
        builder.HandlerType = typeof(AuthenticationHandler);
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Communication API",
        Version = "v1",
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Communication API v1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();