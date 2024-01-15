using IonShard.Contracts.DTO.Map;
using MongoDB.Driver;

namespace IonShard.Persistence.Database;

public interface IShardDatabaseContext
{
    IMongoCollection<PlanetDTO> Objects { get; }

    public Task Test();
}
