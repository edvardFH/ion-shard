using IonShard.Persistence.Repositories;
using IonShard.Services;
using IonShard.Swagger;
using Microsoft.OpenApi.Models;
using Shard.Shared.Core;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

builder.Services.AddControllers();

builder.Services.AddSingleton<MapGenerator>();
builder.Services.Configure<MapGeneratorOptions>(configuration.GetSection("MapGeneratorOptions"));

builder.Services.AddSingleton<MapBuilder>();
builder.Services.AddSingleton<MapRepository>();

builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<UserFactory>();

builder.Services.AddSingleton<SystemClock>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.DocumentFilter<RequestBodiesDocumentFilter>();
    c.EnableAnnotations();
    c.SwaggerDoc(
        configuration.GetValue<string>("AppSettings:Version"),
        new OpenApiInfo
        {
            Version = configuration.GetValue<string>("AppSettings:Version"),
            Title = configuration.GetValue<string>("AppSettings:Title"),
            Description = configuration.GetValue<string>("AppSettings:Description")
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint(
            $"/swagger/{configuration.GetValue<string>("AppSettings:Version")}/swagger.json",
            configuration.GetValue<string>("AppSettings:Title")));
}

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace Shard.IonShard
{
    public partial class Program { }
}