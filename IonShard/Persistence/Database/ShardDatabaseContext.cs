using IonShard.Configuration.Database;
using MongoDB.Driver;

namespace IonShard.Persistence.Database;

public class ShardDatabaseContext: IShardDatabaseContext
{
    private readonly IMongoDatabase _database;

    public ShardDatabaseContext(IDatabaseConfigurationService configurationService)
    {
        var client = new MongoClient(configurationService.Database.ConnectionString);
        _database = client.GetDatabase(configurationService.Database.DatabaseName);
    }

    public IMongoCollection<Object> Objects => _database.GetCollection<Object>("Objects");
}
