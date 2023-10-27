using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;

namespace IonShard.Domain.Units;

public class BuilderUnit : AbstractUnit, IBuilderUnit
{
    public override string Type => "builder";


    public IBuilding Build(string buildingType)
    {
        if (Location.Planet is null)
            throw new Exception();
        
        return new Mine(
            this,
            Location.System,
            Location.Planet);
    }


    public BuilderUnit(StarSystem starSystem, Planet? planet) : base(starSystem, planet) { }
}
