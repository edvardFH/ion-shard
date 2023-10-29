using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using IonShard.Domain.Users;

namespace IonShard.Domain.Units;

public class ScoutUnit : AbstractUnit
{
    public override string Type => "scout";
    public override ILocation Location
        => new LocationWithDetails(base.Location.System, base.Location.Planet);

    public ScoutUnit(IUser owner, StarSystem starSystem, Planet? planet) : base(owner, starSystem, planet) { }
}
