using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;

namespace IonShard.Domain.Units;

public class BuilderUnit: IBuilderUnit
{
    public string Id { get; }
    public string Type => "builder";
    public ILocation Location { get; }


    public IBuilding Build(string buildingType)
    {
        return new Building(
            Guid.NewGuid().ToString(),
            this,
            Location.System,
            Location.Planet);
    }

    
    public BuilderUnit(string id, StarSystem starSystem, Planet? planet)
    {
        Id = id;
        Location = new Location(starSystem, planet);
    }
}
