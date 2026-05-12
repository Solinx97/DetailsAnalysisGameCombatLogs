using Chat.Application.Consts;
using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Helpers;
using CombatAnalysis.Hubs.Hubs;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();

builder.Services.Configure<Cluster>(builder.Configuration.GetSection("Cluster"));

var authenticationOptions = new Authentication();
builder.Configuration.Bind("Authentication", authenticationOptions);
var authenticationClientOptions = new AuthenticationClient();
builder.Configuration.Bind("Authentication:Client", authenticationClientOptions);

var keySet = await JwtBearerOptionsHelper.GetJWKSAsync(authenticationOptions.Authority);
var audiences = authenticationClientOptions.Audiences.Split(',');
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
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

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.AccessToken), out var accessToken))
                {
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && MessageReceivedHelper.IsHubExist(path))
                    {
                        context.Token = accessToken;
                    }
                }

                return Task.CompletedTask;
            }
        };

        // Skip checking HTTPS (should be HTTPS in production)
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddSingleton<IKafkaProducerService<string, string>, KafkaProducer<string, string>>();

var cors = new CORS();
builder.Configuration.Bind("Cors", cors);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins(cors.WebApp)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

builder.Services.AddSignalR()
        .AddJsonProtocol();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
    .WriteTo.File("logs/hubs.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseRouting()
   .UseAuthentication()
   .UseAuthorization()
   .UseEndpoints(endpoints =>
{
    app.MapHub<PersonalChatHub>(HubPatterns.PersonalChat);
    app.MapHub<PersonalChatMessagesHub>(HubPatterns.PersonalChatMessages);
    app.MapHub<PersonalChatUnreadMessageHub>(HubPatterns.PersonalChatUnreadMessage);
    app.MapHub<GroupChatHub>(HubPatterns.GroupChat);
    app.MapHub<GroupChatMessagesHub>(HubPatterns.GroupChatMessages);
    app.MapHub<GroupChatUnreadMessageHub>(HubPatterns.GroupChatUnreadMessage);
    app.MapHub<VoiceChatHub>(HubPatterns.VoiceChat);
    app.MapHub<NotificationHub>(HubPatterns.Notification);
});

app.UseHttpsRedirection();

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
