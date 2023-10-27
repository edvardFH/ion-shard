using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using IonShard.Domain.Units;
using IonShard.Utils;

namespace IonShard.Domain.Buildings;

public class Mine: IBuilding
{
    public string Id { get; }

    public string Type => "mine";

    public IUnit Builder {  get; }

    public ILocation Location { get; }

    public Mine(IUnit builder, StarSystem starSystem, Planet? planet)
    {
        Id = new Random().NextGuid().ToString();
        Builder = builder;
        Location = new Location(starSystem, planet);
    }
}
