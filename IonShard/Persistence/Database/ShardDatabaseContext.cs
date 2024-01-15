using IonShard.Contracts.DTO.Map;
using MongoDB.Driver;

namespace IonShard.Persistence.Database;

public class ShardDatabaseContext: IShardDatabaseContext
{
    const string environmentVariableName = "mongo_connection";
    private readonly IMongoDatabase _database;

    public ShardDatabaseContext()
    {
        var connectionString = Environment.GetEnvironmentVariable(environmentVariableName);

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("sharddb");
    }

    public IMongoCollection<PlanetDTO> Objects => _database.GetCollection<PlanetDTO>("Objects");

    public async Task Test()
    {
        var collection = _database.GetCollection<PlanetDTO>("Objects");
        var entries = await collection.Find(_ => true).ToListAsync();

        entries.ToList().ForEach(entry => Console.WriteLine(entry));
    }
}
