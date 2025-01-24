using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Helpers;
using CombatAnalysis.Hubs.Hubs;
using CombatAnalysis.Hubs.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();

var webAppCors = builder.Configuration["Cors:WebApp"] ?? string.Empty;

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins(webAppCors)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

builder.Services.AddSignalR()
        .AddJsonProtocol();

builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer(options =>
        {
            options.Authority = builder.Configuration["Authentication:Authority"];
            options.Audience = builder.Configuration["Authentication:Audience"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
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
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

Port.ChatApi = builder.Configuration["ChatApiPort"] ?? string.Empty;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();

app.UseWebSockets();

app.UseCors("CorsPolicy");

app.UseRouting().UseEndpoints(endpoints =>
{
    app.MapHub<PersonalChatHub>("/personalChatHub");
    app.MapHub<PersonalChatMessagesHub>("/personalChatMessagesHub");
    app.MapHub<PersonalChatUnreadMessageHub>("/personalChatUnreadMessageHub");
    app.MapHub<GroupChatHub>("/groupChatHub");
    app.MapHub<GroupChatMessagesHub>("/groupChatMessagesHub");
    app.MapHub<GroupChatUnreadMessageHub>("/groupChatUnreadMessageHub");
    app.MapHub<VoiceChatHub>("/voiceChatHub");
});

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();
