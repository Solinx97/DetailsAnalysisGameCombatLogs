using AutoMapper;
using Chat.Application.Extensions;
using Chat.Application.Mappers;
using Chat.Infrastructure.Extensions;
using Chat.Infrastructure.Mappers;
using CombatAnalysis.ChatAPI.BackgroundServices;
using CombatAnalysis.ChatAPI.Consts;
using CombatAnalysis.ChatAPI.Helpers;
using CombatAnalysis.ChatAPI.Hubs;
using CombatAnalysis.ChatAPI.Interfaces;
using CombatAnalysis.ChatAPI.Kafka;
using CombatAnalysis.ChatAPI.Kafka.Producer;
using CombatAnalysis.ChatAPI.Mapping;
using CombatAnalysis.ChatAPI.Providers;
using CombatAnalysis.ChatAPI.Services;
using MessageBroker.Kafka.Consts;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// For testing secrets
builder.Configuration.AddUserSecrets<Program>();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

var databasePropsOptions = new DatabaseProps();
builder.Configuration.Bind("Database", databasePropsOptions);

builder.Services.AddChatApplication();
builder.Services.AddChatInfrastructure(databasePropsOptions.DefaultConnection);

var loggerFactory = LoggerFactory.Create(builder => { });

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new ChatMapper());
    mc.AddProfile(new ApplicationChatProfile());
    mc.AddProfile(new InfrastructureChatProfile());
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
        var keySet = JwtBearerOptionsHelper.GetJWKSAsync(authenticationOptions.Authority).GetAwaiter().GetResult();

        options.Authority = authenticationOptions.Authority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authenticationOptions.Issuer,
            ValidateAudience = true,
            ValidAudiences = audiences,
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = keySet.Keys,
            ClockSkew = TimeSpan.Zero,
        };

        // Skip checking HTTPS (should be HTTPS in production)
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("ApiScope", builder =>
    {
        builder.RequireAuthenticatedUser();
        builder.RequireClaim("scope", authenticationClientOptions.Scopes.Split(','));
    });

builder.Services.AddSignalR()
        .AddJsonProtocol();

builder.Services.AddHostedService<PersonalChatConsumer>();
builder.Services.AddHostedService<PersonalChatMessageConsumer>();
builder.Services.AddHostedService<GroupChatConsumer>();
builder.Services.AddHostedService<GroupChatMemberConsumer>();
builder.Services.AddHostedService<GroupChatMessageConsumer>();

builder.Services.AddHostedService<OutboxWorker>();

builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Chat API",
        Version = "v1",
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your access token.\nExample: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddSingleton<ILiveKitService, LiveKitService>();
builder.Services
    .AddOptions<LiveKitOptions>()
    .BindConfiguration(LiveKitOptions.SectionName)
    .Validate(
        x => !string.IsNullOrWhiteSpace(x.Url)
          && !string.IsNullOrWhiteSpace(x.ApiKey)
          && !string.IsNullOrWhiteSpace(x.ApiSecret),
        "LiveKit configuration is incomplete.")
    .ValidateOnStart();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
    .WriteTo.File("logs/chatapi.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<PersonalChatHub>(HubPatterns.PERSONAL_CHAT);
app.MapHub<PersonalChatMessagesHub>(HubPatterns.PERSONAL_CHAT_MESSAGE);
app.MapHub<PersonalChatUnreadMessagesHub>(HubPatterns.PERSONAL_CHAT_UNREAD_MESSAGES);
app.MapHub<GroupChatHub>(HubPatterns.GROUP_CHAT);
app.MapHub<GroupChatMessagesHub>(HubPatterns.GROUP_CHAT_MESSAGE);
app.MapHub<GroupChatUnreadMessagesHub>(HubPatterns.GROUP_CHAT_UNREAD_MESSAGES);
app.MapHub<VoiceChatHub>(HubPatterns.VOICE_CHAT);

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat API v1");
    options.InjectStylesheet("/swagger-ui/swaggerDark.css");
});

app.UseStaticFiles();

app.MapControllers().RequireAuthorization("ApiScope");

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var ex = exceptionHandlerPathFeature?.Error;

        Log.Error(ex, "Unhandled exception occurred");

        var result = new
        {
            message = "An unexpected error occurred. Please try again later."
        };

        await context.Response.WriteAsJsonAsync(result);
    });
});

app.Run();