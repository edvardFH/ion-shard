using IonShard.Contracts.DTO.Map;
using IonShard.Domain.Map;
using MongoDB.Driver;

namespace IonShard.Persistence.Database;

public interface IShardDatabaseService
{
    public IReadOnlyList<StarSystem> GetMap();
    public Task UpdateMap(IEnumerable<StarSystem> map);
}
