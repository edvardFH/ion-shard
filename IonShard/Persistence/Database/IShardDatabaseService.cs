using IonShard.Contracts.DTO.Map;
using IonShard.Domain.Map;
using IonShard.Domain.Users;
using MongoDB.Driver;

namespace IonShard.Persistence.Database;

public interface IShardDatabaseService
{
    public IReadOnlyList<StarSystem> GetMap();
    public Task UpdateMapAsync(IEnumerable<StarSystem> map);

    public IReadOnlyList<IUser> GetUsers();
    public Task UpdateUsersAsync(IEnumerable<IUser> users);
}
