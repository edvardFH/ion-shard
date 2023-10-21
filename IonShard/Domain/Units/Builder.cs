using IonShard.Domain.Buildings;
using IonShard.Domain.Map;

namespace IonShard.Domain.Units;

public class Builder: IBuilder
{
    public string Id { get; }
    public string Type => "builder";
    public Location Location { get; }

    public Building Build(string buildingType)
    {
        return new Building(
            Guid.NewGuid().ToString(),
            this,
            Location.System,
            Location.Planet);
    }

    public Builder(string id, StarSystem starSystem, Planet? planet)
    {
        Id = id;
        Location = new(starSystem, planet);
    }
}
