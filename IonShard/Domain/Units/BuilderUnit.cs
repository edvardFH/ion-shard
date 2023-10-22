using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;

namespace IonShard.Domain.Units;

public class BuilderUnit : AbstractUnit, IBuilderUnit
{
    public override string Type => "builder";


    public IBuilding Build(string buildingType)
    {
        return new Building(
            Guid.NewGuid().ToString(),
            this,
            Location.System,
            Location.Planet);
    }


    public BuilderUnit(string id, StarSystem starSystem, Planet? planet) : base(id, starSystem, planet) { }
}
