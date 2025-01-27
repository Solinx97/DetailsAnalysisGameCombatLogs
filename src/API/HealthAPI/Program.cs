using AutoMapper;
using CombatAnalysis.Identity.Extensions;
using CombatAnalysis.Identity.Mapping;
using HealthAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterIdentityDependencies(builder.Configuration, "DefaultConnection");

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new IdentityMapper());
});
var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddHostedService<RefreshTokenCleanupService>();
builder.Services.AddHostedService<AuthCodeCleanupService>();
builder.Services.AddHostedService<VerificationEmailService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Health API v1");
    options.InjectStylesheet("/swagger-ui/swaggerDark.css");
    options.OAuthClientId(builder.Configuration["Client:ClientId"]);
    options.OAuthClientSecret(builder.Configuration["Client:ClientSecret"]);
});

app.UseStaticFiles();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
