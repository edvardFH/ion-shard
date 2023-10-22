using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;

namespace IonShard.Domain.Units;

public class ScoutUnit : IUnit
{
    public string Id { get; }
    public string Type => "scout";
    public ILocation Location { get; }


    public ScoutUnit(string id, StarSystem starSystem, Planet? planet)
    {
        Id = id;
        Location = new LocationWithDetails(starSystem, planet);
    }
}
