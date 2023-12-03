using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using IonShard.Domain.Users;

namespace IonShard.Domain.Units.Scout;

public class ScoutUnit : Unit, IScoutUnit
{
    public ScoutUnit(IUser owner, StarSystem starSystem, Planet? planet) : base(owner, starSystem, planet, "Scout") { }

    public override ILocationWithDetails Location
        => new LocationWithDetails(base.Location.System, base.Location.Planet);
}
