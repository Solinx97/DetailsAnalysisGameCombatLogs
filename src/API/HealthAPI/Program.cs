using AutoMapper;
using CombatAnalysis.Identity.Extensions;
using CombatAnalysis.Identity.Mapping;
using HealthAPI.Consts;
using HealthAPI.Extensions;
using HealthAPI.Services;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var databasePropsOptions = new DatabaseProps();
builder.Configuration.Bind("Database", databasePropsOptions);

builder.Services.RegisterIdentityDependencies(databasePropsOptions.DefaultConnection);

var loggerFactory = LoggerFactory.Create(builder => { });

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new IdentityMapper());
}, loggerFactory);

var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var authenticationOptions = new Authentication();
builder.Configuration.Bind("Authentication", authenticationOptions);
var authenticationClientOptions = new AuthenticationClient();
builder.Configuration.Bind("Authentication:Client", authenticationClientOptions);

var audiences = authenticationClientOptions.Audiences.Split(',');
builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer(options =>
        {
            var keySet = options.GetJWKSAsync(authenticationOptions.Authority).GetAwaiter().GetResult();

            options.Authority = authenticationOptions.Authority;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = authenticationOptions.Issuer,
                ValidateAudience = true,
                ValidAudiences = audiences,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = keySet.Keys,
                ClockSkew = TimeSpan.Zero
            };
            // Skip checking HTTPS (should be HTTPS in production)
            options.RequireHttpsMetadata = false;
        });

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
    //options.OAuthClientId(authenticationClientOptions.Audiences);
    //options.OAuthScopes(authenticationClientOptions.Scopes);
});

app.UseStaticFiles();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
