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
        _mapCollection.InsertOne(
            new StarSystem
            (
                "13ed60e3-1692-56cd-ae38-b7c02013ce9e",
                new List<Planet>()
            ).ToPOCO());

        return _mapMapper.SystemPocoToUniverse(_mapCollection.Find(_ => true).ToList());
    }


    public async Task UpdateMap(IEnumerable<StarSystem> map)
    {
        var bulkOperation = new List<WriteModel<StarSystemPOCO>>();

        map.ToList().ForEach(starSystem =>
        {
            var starSystemPoco = starSystem.ToPOCO();
            var filter = Builders<StarSystemPOCO>.Filter.Eq("Id", starSystemPoco.Name);
            var upsertOne = new ReplaceOneModel<StarSystemPOCO>(filter, starSystemPoco) { IsUpsert = true };

            bulkOperation.Add(upsertOne);
        });

        await _mapCollection.BulkWriteAsync(bulkOperation);
    }
}
