using IonShard.Configuration;
using IonShard.Persistence.Repositories;
using IonShard.Services;
using IonShard.Swagger;
using Microsoft.OpenApi.Models;
using Shard.Shared.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("Configuration/gamerules.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();

builder.Services.AddSingleton<GameRulesConfigurationService>();

builder.Services.AddSingleton<MapGenerator>();
builder.Services.Configure<MapGeneratorOptions>(
    builder.Configuration.GetSection("MapGeneratorOptions"));

builder.Services.AddSingleton<MapBuilder>();
builder.Services.AddSingleton<MapRepository>();

builder.Services.AddSingleton<UnitFactory>();

builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<UserFactory>();


builder.Services.AddSingleton<SystemClock>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.DocumentFilter<RequestBodiesDocumentFilter>();
    c.EnableAnnotations();
    c.SwaggerDoc(
        builder.Configuration.GetValue<string>("AppSettings:Version"),
        new OpenApiInfo
        {
            Version = builder.Configuration.GetValue<string>("AppSettings:Version"),
            Title = builder.Configuration.GetValue<string>("AppSettings:Title"),
            Description = builder.Configuration.GetValue<string>("AppSettings:Description")
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint(
            $"/swagger/{builder.Configuration.GetValue<string>("AppSettings:Version")}/swagger.json",
            builder.Configuration
                .GetValue<string>("AppSettings:Title")));
}

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace Shard.IonShard
{
    public partial class Program { }
}