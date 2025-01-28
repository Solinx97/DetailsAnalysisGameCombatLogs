using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Helpers;
using CombatAnalysis.Hubs.Hubs;
using CombatAnalysis.Hubs.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();

var envName = builder.Environment.EnvironmentName;

if (string.Equals(envName, "Development", StringComparison.OrdinalIgnoreCase))
{
    CreateEnvironmentHelper.UseAppsettings(builder.Configuration);
}
else
{
    CreateEnvironmentHelper.UseEnvVariables();
}

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = Authentication.Authority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Authentication.IssuerSigningKey),
            ValidateIssuer = true,
            ValidIssuer = Authentication.Issuer,
            ValidateAudience = true,
            ValidAudiences = [AuthenticationClient.WebClientId, AuthenticationClient.DesktopClientId],
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins(CORS.WebApp)
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

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();

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
