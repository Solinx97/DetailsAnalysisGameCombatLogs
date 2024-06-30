using AutoMapper;
using CombatAnalysis.CustomerBL.Extensions;
using CombatAnalysis.CustomerBL.Mapping;
using CombatAnalysis.Identity.Extensions;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Settings;
using CombatAnalysis.UserApi.Mapping;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.CustomerBLDependencies(builder.Configuration, "DefaultConnection");
//builder.Services.RegisterIdentityDependencies();

//var settings = builder.Configuration.GetSection(nameof(TokenSettings));
//var scheme = settings.GetValue<string>(nameof(TokenSettings.AuthScheme));

//builder.Services.Configure<TokenSettings>(settings);

var loggerFactory = new LoggerFactory();
var logger = new Logger<ILogger>(loggerFactory);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new UserApiMapper());
    mc.AddProfile(new CustomerBLMapper());
});

var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddSingleton<ILogger>(logger);
builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = builder.Configuration["Authentication:identityServer"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false
            };
        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.RequireClaim("scope", builder.Configuration["Client:Scope"] ?? string.Empty);
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User API",
        Version = "v1",
    });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri($"{builder.Configuration["Authentication:identityServer"]}/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { builder.Configuration["Client:Scope"] ?? string.Empty, "Request API #1"}
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
                new[] { builder.Configuration["Client:Scope"] }
            }
        });
});

var app = builder.Build();

//var scope = app.Services.CreateScope();
//var jwtSecretService = scope.ServiceProvider.GetService<IJWTSecret>();
//await jwtSecretService.GenerateSecretKeysAsync();

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "User API v1");
    options.InjectStylesheet("/swagger-ui/swaggerDark.css");
    options.OAuthClientId(builder.Configuration["Client:ClientId"]);
    options.OAuthClientSecret(builder.Configuration["Client:ClientSecret"]);
});

app.UseStaticFiles();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();