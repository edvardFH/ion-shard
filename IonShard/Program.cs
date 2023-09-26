using IonShard.Services;
using Microsoft.OpenApi.Models;
using Shard.Shared.Core;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSingleton<MapGenerator>();
builder.Services.Configure<MapGeneratorOptions>(configuration.GetSection("MapGeneratorOptions"));
builder.Services.AddSingleton<MapBuilder>();
builder.Services.AddSingleton<MapRepository>();

//builder.Services.AddSingleton<UsersRepository>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc(configuration.GetValue<string>("AppSettings:Version"), new OpenApiInfo 
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
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace Shard.IonShard
{
    public partial class Program { }
}