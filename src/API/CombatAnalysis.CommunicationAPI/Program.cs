using AutoMapper;
using CombatAnalysis.CommunicationAPI.Mapping;
using CombatAnalysis.CommunicationBL.Extensions;
using CombatAnalysis.CommunicationBL.Mapping;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.CommunicationBLDependencies(builder.Configuration, "DefaultConnection");

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new CommunicationMapper());
    mc.AddProfile(new BLMapper());
});

var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer(options =>
        {
            options.Authority = builder.Configuration["Authentication:Authority"];
            options.Audience = builder.Configuration["Authentication:Audience"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Authentication:IssuerSigningKey"] ?? string.Empty)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
            // Skip checking HTTPS (should be HTTPS in production)
            options.RequireHttpsMetadata = false;
            // Allow all Certificates (added for Local deployment)
            options.BackchannelHttpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Communication API",
        Version = "v1",
    });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri($"{builder.Configuration["Identity:Server"]}connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { builder.Configuration["Client:Scope"] ?? string.Empty, "Request API #1" }
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

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Communication API v1");
        options.InjectStylesheet("/swagger-ui/swaggerDark.css");
        options.OAuthClientId(builder.Configuration["Client:ClientId"]);
        options.OAuthClientSecret(builder.Configuration["Client:ClientSecret"]);
    });
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();