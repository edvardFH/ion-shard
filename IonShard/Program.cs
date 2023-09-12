using IonShard.Services;
using Shard.Shared.Core;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<MapGenerator>();
builder.Services.Configure<MapGeneratorOptions>(
    configuration.GetSection("MapGeneratorOptions"));

builder.Services.AddSingleton<MapBuilderService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace Shard.IonShard
{
    public partial class Program { }
}