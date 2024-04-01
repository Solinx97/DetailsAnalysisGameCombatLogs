using AutoMapper;
using CombatAnalysis.ChatBL.Extensions;
using CombatAnalysis.CustomerBL.Extensions;
using CombatAnalysis.ChatBL.Mapping;
using CombatAnalysis.ChatApi.Mapping;
using CombatAnalysis.ChatApi.Middleware;
using CombatAnalysis.Identity.Extensions;
using CombatAnalysis.Identity.Settings;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ChatBLDependencies(builder.Configuration, "DefaultConnection");
builder.Services.CustomerBLDependencies(builder.Configuration, "UserConnection");
builder.Services.RegisterIdentityDependencies();

var settings = builder.Configuration.GetSection(nameof(TokenSettings));
var scheme = settings.GetValue<string>(nameof(TokenSettings.AuthScheme));

builder.Services.Configure<TokenSettings>(settings);

var loggerFactory = new LoggerFactory();
var logger = new Logger<ILogger>(loggerFactory);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new ChatMapper());
    mc.AddProfile(new ChatBLMapper());
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
        Title = "Chat API",
        Version = "v1",
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat API v1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();