using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Users;
using Shard.Shared.Core;

namespace IonShard.Domain.Units.Scout;

public class ScoutUnit : Unit, IScoutUnit
{
    public ScoutUnit
        (
            string id,
            IUser owner,
            StarSystem starSystem,
            Planet? planet,
            IReadOnlyDictionary<IResource, int> resourceCost,
            IClock clock
        )
        : base(id, owner, starSystem, planet, "Scout", resourceCost, clock) { }

    public override ILocationWithDetails Location
        => new LocationWithDetails(base.Location.System, base.Location.Planet);
}
