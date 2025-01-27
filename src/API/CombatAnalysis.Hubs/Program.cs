using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Helpers;
using CombatAnalysis.Hubs.Hubs;
using CombatAnalysis.Hubs.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();

var webAppCors = string.Empty;
var envName = builder.Environment.EnvironmentName;

if (string.Equals(envName, "Development", StringComparison.OrdinalIgnoreCase))
{
    webAppCors = builder.Configuration["Cors:WebApp"] ?? string.Empty;
    API.Chat = builder.Configuration["API:Chat"] ?? string.Empty;
}
else
{
    webAppCors = Environment.GetEnvironmentVariable("Cors_WebApp") ?? string.Empty;
    API.Chat = Environment.GetEnvironmentVariable("API_Chat") ?? string.Empty;
}

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

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();

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

app.UseHttpsRedirection();
app.Run();
