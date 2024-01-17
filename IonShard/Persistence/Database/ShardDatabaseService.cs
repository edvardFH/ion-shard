using IonShard.Adapters.Mappers;
using IonShard.Domain.Map;
using IonShard.Domain.Users;
using IonShard.Persistence.POCOs;
using MongoDB.Driver;

namespace IonShard.Persistence.Database;

public class ShardDatabaseService: IShardDatabaseService
{
    const string EnvironmentVariableName = "mongo_connection";
    const string DatabaseName = "sharddb";
    const string MapCollectionName = "map";
    const string UserCollectionName = "users";

    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<StarSystemPOCO> _mapCollection;
    private readonly IMongoCollection<UserPOCO> _userCollection;

    private readonly MapMapper _mapMapper;
    private readonly UserMapper _userMapper;


    public ShardDatabaseService(MapMapper mapMapper, UserMapper userMapper)
    {
        var connectionString = Environment.GetEnvironmentVariable(EnvironmentVariableName);

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(DatabaseName);
        _mapCollection = _database.GetCollection<StarSystemPOCO>(MapCollectionName);
        _userCollection = _database.GetCollection<UserPOCO>(UserCollectionName);
        _mapMapper = mapMapper;
        _userMapper = userMapper;
    }


    public IReadOnlyList<StarSystem> GetMap()
        => _mapMapper.SystemPocoToUniverse(_mapCollection.Find(_ => true).ToList());


    public async Task UpdateMapAsync(IEnumerable<StarSystem> map)
    {
        await _mapCollection.BulkWriteAsync(InitMapBulkOperation(map));
    }


    public IReadOnlyList<IUser> GetUsers()
        => _userCollection
            .Find(_ => true)
            .ToList()
            .Select(_userMapper.UserPOCOToDomain)
            .ToList();

    public async Task UpdateUsersAsync(IEnumerable<IUser> users)
    {
        await _userCollection.BulkWriteAsync(InitUserBuilkOperation(users));
    }


    private List<WriteModel<StarSystemPOCO>> InitMapBulkOperation(IEnumerable<StarSystem> map)
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


    private List<WriteModel<UserPOCO>> InitUserBuilkOperation(IEnumerable<IUser> users)
    {
        var bulkOperation = new List<WriteModel<UserPOCO>>();

        users.ToList().ForEach(user =>
        {
            var userPOCO = user.ToPOCO();
            var filter = Builders<UserPOCO>.Filter.Eq("Id", userPOCO.Id);
            var upsertOne = new ReplaceOneModel<UserPOCO>(filter, userPOCO) { IsUpsert = true };

            bulkOperation.Add(upsertOne);

        });

        return bulkOperation;
    }
}
