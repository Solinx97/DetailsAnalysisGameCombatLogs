using AutoMapper;
using CombatAnalysis.CustomerBL.Extensions;
using CombatAnalysis.CustomerBL.Mapping;
using CombatAnalysis.Identity.Extensions;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Mapping;
using CombatAnalysis.Identity.Settings;
using CombatAnalysis.UserApi.Mapping;
using CombatAnalysis.UserApi.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.CustomerBLDependencies(builder.Configuration, "DefaultConnection");
builder.Services.RegisterIdentityDependencies();

var settings = builder.Configuration.GetSection(nameof(TokenSettings));
var scheme = settings.GetValue<string>(nameof(TokenSettings.AuthScheme));

builder.Services.Configure<TokenSettings>(settings);

var loggerFactory = new LoggerFactory();
var logger = new Logger<ILogger>(loggerFactory);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new UserApiMapper());
    mc.AddProfile(new CustomerBLMapper());
    mc.AddProfile(new IdentityMappingMapper());
});

var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddSingleton<ILogger>(logger);
builder.Services.AddAuthentication("Basic")
    .AddScheme<BasicAuthenticationOptions, AuthenticationHandler>("Basic", null);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User API",
        Version = "v1",
    });
});

var app = builder.Build();

var scope = app.Services.CreateScope();
var jwtSecretService = scope.ServiceProvider.GetService<IJWTSecret>();
await jwtSecretService.GenerateSecretKeysAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "User API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();