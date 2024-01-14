using MongoDB.Driver;

namespace IonShard.Persistence.Database;

public interface IShardDatabaseContext
{
    IMongoCollection<Object> Objects { get; }
}
