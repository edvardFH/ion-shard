using IonShard.Contracts.DTO.Map;
using IonShard.Domain.Map;
using MongoDB.Driver;

namespace IonShard.Persistence.Database;

public class ShardDatabaseService: IShardDatabaseService
{
    const string environmentVariableName = "mongo_connection";
    private readonly IMongoDatabase _database;

    public ShardDatabaseService()
    {
        var connectionString = Environment.GetEnvironmentVariable(environmentVariableName);

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("sharddb");
    }

    public IReadOnlyList<StarSystem> GetMap()
    {
        var collection = _database.GetCollection<StarSystem>("map");
        var entries = collection.Find(_ => true).ToList();

        entries.ToList().ForEach(entry => Console.WriteLine(entry));

        return new List<StarSystem>();
    }
}
