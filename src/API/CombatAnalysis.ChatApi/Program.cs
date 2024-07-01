using AutoMapper;
using CombatAnalysis.ChatApi.Mapping;
using CombatAnalysis.ChatBL.Extensions;
using CombatAnalysis.ChatBL.Mapping;
using CombatAnalysis.CustomerBL.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ChatBLDependencies(builder.Configuration, "DefaultConnection");
builder.Services.CustomerBLDependencies(builder.Configuration, "UserConnection");

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
builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = builder.Configuration["Authentication:IdentityServer"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Authentication:IssuerSigningKey"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
                TokenUrl = new Uri($"{builder.Configuration["Authentication:IdentityServer"]}/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "api1", "Request API #1"}
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
                new[] { "api1" }
            }
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat API v1");
    options.InjectStylesheet("/swagger-ui/swaggerDark.css");
    options.OAuthClientId("client1"); // Replace with your client ID
    options.OAuthClientSecret("secret1"); // Replace with your client secret
});

app.UseStaticFiles();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();