using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;

namespace IonShard.Domain.Units;

public class ScoutUnit : AbstractUnit
{
    public override string Type => "scout";
    public override ILocation Location
        => new LocationWithDetails(base.Location.System, base.Location.Planet);

    public ScoutUnit(string id, StarSystem starSystem, Planet? planet) : base(id, starSystem, planet) { }
}
