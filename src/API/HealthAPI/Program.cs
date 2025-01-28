using AutoMapper;
using CombatAnalysis.Identity.Extensions;
using CombatAnalysis.Identity.Mapping;
using HealthAPI.Consts;
using HealthAPI.Helpers;
using HealthAPI.Services;
using Microsoft.IdentityModel.Tokens;

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

builder.Services.RegisterIdentityDependencies(DatabaseProps.ConnectionString);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new IdentityMapper());
});

var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer(options =>
        {
            options.Authority = Authentication.Authority;
            options.Audience = AuthenticationClient.ClientId;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Authentication.IssuerSigningKey),
                ValidateIssuer = true,
                ValidIssuer = Authentication.Issuer,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };
            // Skip checking HTTPS (should be HTTPS in production)
            options.RequireHttpsMetadata = false;
        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.RequireClaim("scope", AuthenticationClient.Scope);
    });
});

builder.Services.AddHostedService<RefreshTokenCleanupService>();
builder.Services.AddHostedService<AuthCodeCleanupService>();
builder.Services.AddHostedService<VerificationEmailService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Health API v1");
    options.InjectStylesheet("/swagger-ui/swaggerDark.css");
    options.OAuthClientId(AuthenticationClient.ClientId);
    options.OAuthScopes(AuthenticationClient.Scope);
});

app.UseStaticFiles();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
