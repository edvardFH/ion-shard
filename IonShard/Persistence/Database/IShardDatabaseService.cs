using IonShard.Contracts.DTO.Map;
using IonShard.Domain.Map;
using MongoDB.Driver;

namespace IonShard.Persistence.Database;

public interface IShardDatabaseService
{
    public IReadOnlyList<StarSystem> GetMap();
    public Task UpdateMapAsync(IEnumerable<StarSystem> map);

    public void UpdateMap(IEnumerable<StarSystem> map);
}
