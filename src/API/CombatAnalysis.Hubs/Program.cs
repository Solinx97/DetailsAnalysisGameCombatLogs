using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Helpers;
using CombatAnalysis.Hubs.Hubs;
using CombatAnalysis.Hubs.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyMethod()
               .AllowAnyHeader()
               .WithOrigins(
                    "https://localhost:7026", 
                    "https://localhost:44479"
                )
               .AllowCredentials();
    });
});

builder.Services.AddSignalR();

Port.ChatApi = builder.Configuration["ChatApiPort"] ?? string.Empty;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseRouting();

app.MapHub<ChatHub>("/chatHub");

app.Run();
