using AutoMapper;
using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Helpers;
using CombatAnalysis.ChatApi.Mapping;
using CombatAnalysis.ChatBL.Extensions;
using CombatAnalysis.ChatBL.Mapping;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.ChatBLDependencies(DatabaseProps.Name, DatabaseProps.DataProcessingType, connection);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new ChatMapper());
    mc.AddProfile(new ChatBLMapper());
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
    options.AddPolicy("ApiScope", builder =>
    {
        builder.RequireAuthenticatedUser();
        builder.RequireClaim("scope", "api1");
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Chat API",
        Version = "v1",
    });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri($"{API.Identity}connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { AuthenticationClient.Scope, "Request API #1" }
                }
            }
        }
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "oauth2"
                    },
                },
                new[] { AuthenticationClient.Scope }
            }
        });
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
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
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat API v1");
    options.InjectStylesheet("/swagger-ui/swaggerDark.css");
    options.OAuthClientId(AuthenticationClient.ClientId);
    options.OAuthScopes(AuthenticationClient.Scope);
});

app.UseStaticFiles();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();