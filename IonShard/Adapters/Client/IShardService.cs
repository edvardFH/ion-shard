using IonShard.Configuration.Wormholes;
using IonShard.Domain.Units;
using IonShard.Domain.Users;

namespace IonShard.Adapters.Client;

public interface IShardService
{
    public Task<Uri> PutUnitAsync(WormholeConfig wormhole, IUnit unit);
}
