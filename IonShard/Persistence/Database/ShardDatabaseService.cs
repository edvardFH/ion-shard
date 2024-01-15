using IonShard.Adapters.Mappers;
using IonShard.Domain.Map;
using IonShard.Persistence.POCOs;
using MongoDB.Driver;

namespace IonShard.Persistence.Database;

public class ShardDatabaseService: IShardDatabaseService
{
    const string EnvironmentVariableName = "mongo_connection";
    const string DatabaseName = "sharddb";
    const string MapCollectionName = "map";

    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<StarSystemPOCO> _mapCollection;
    private readonly MapMapper _mapMapper;

    public ShardDatabaseService(MapMapper mapMapper)
    {
        var connectionString = Environment.GetEnvironmentVariable(EnvironmentVariableName);

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(DatabaseName);
        _mapCollection = _database.GetCollection<StarSystemPOCO>(MapCollectionName);
        _mapMapper = mapMapper;
    }

    public IReadOnlyList<StarSystem> GetMap()
    {
        return _mapMapper.SystemPocoToUniverse(_mapCollection.Find(_ => true).ToList());
    }


    public async Task UpdateMapAsync(IEnumerable<StarSystem> map)
    {
        await _mapCollection.BulkWriteAsync(InitBulkOperation(map));
    }

    public void UpdateMap(IEnumerable<StarSystem> map)
    {
        _mapCollection.BulkWrite(InitBulkOperation(map));
    }

    private List<WriteModel<StarSystemPOCO>> InitBulkOperation(IEnumerable<StarSystem> map)
    {
        var bulkOperation = new List<WriteModel<StarSystemPOCO>>();

        map.ToList().ForEach(starSystem =>
        {
            var starSystemPoco = starSystem.ToPOCO();
            var filter = Builders<StarSystemPOCO>.Filter.Eq("Name", starSystemPoco.Name);
            var upsertOne = new ReplaceOneModel<StarSystemPOCO>(filter, starSystemPoco) { IsUpsert = true };

            bulkOperation.Add(upsertOne);
        });

        return bulkOperation;
    }
}
