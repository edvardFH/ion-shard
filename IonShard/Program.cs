using IonShard.Adapters.Client;
using IonShard.Application;
using IonShard.Application.Authentication;
using IonShard.Configuration.Database;
using IonShard.Configuration.Gamerules;
using IonShard.Configuration.Wormholes;
using IonShard.Domain.Buildings;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units;
using IonShard.Persistence.Database;
using IonShard.Persistence.Repositories;
using IonShard.Swagger;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using Shard.Shared.Core;
using SystemClock = Shard.Shared.Core.SystemClock;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
            .AddJsonFile("Configuration/appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("Configuration/Gamerules/gamerules.json", optional: false, reloadOnChange: true)
            .AddJsonFile("Configuration/Authentication/users.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();

builder.Services.AddSingleton<IWormholesConfigService, WormholesConfigService>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IGameRulesService, GameRulesService>();
builder.Services.AddSingleton<IDatabaseConfigurationService, DatabaseConfigurationService>();

builder.Services.AddSingleton<IShardDatabaseContext, ShardDatabaseContext>();

builder.Services.AddHttpClient<IShardService, ShardService>();
builder.Services
    .AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, ShardAuthenticationHandler>("Basic", null);

builder.Services.AddSingleton<IResourceFactory, ResourceFactory>();

builder.Services.AddSingleton<MapGenerator>();
builder.Services.Configure<MapGeneratorOptions>(
    builder.Configuration.GetSection("MapGeneratorOptions"));

builder.Services.AddSingleton<MapBuilder>();
builder.Services.AddSingleton<MapRepository>();

builder.Services.AddSingleton<IUnitFactory, UnitFactory>();
builder.Services.AddSingleton<IBuildingFactory, BuildingFactory>();

builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<IUserFactory, UserFactory>();


builder.Services.AddSingleton<IClock, SystemClock>();


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

namespace Shard.IonShard
{
    public partial class Program { }
}