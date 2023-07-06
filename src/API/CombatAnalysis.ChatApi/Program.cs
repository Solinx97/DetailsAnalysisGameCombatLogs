using AutoMapper;
using CombatAnalysis.BL.Extensions;
using CombatAnalysis.BL.Mapping;
using CombatAnalysis.ChatApi.Mapping;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.BLDependencies(builder.Configuration, "DefaultConnection");

var loggerFactory = new LoggerFactory();
var logger = new Logger<ILogger>(loggerFactory);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new ChatMapper());
    mc.AddProfile(new BLMapper());
});

var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddSingleton<ILogger>(logger);

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
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();