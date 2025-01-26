using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Helpers;
using CombatAnalysis.Hubs.Hubs;
using CombatAnalysis.Hubs.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

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

Port.ChatApi = builder.Configuration["ChatApiPort"] ?? string.Empty;

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
