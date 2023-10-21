using IonShard.Domain.Map;
using IonShard.Domain.Units;

namespace IonShard.Domain.Buildings;

public class Building
{
    public string Id { get; }

    public string Type => "mine";

    public IUnit Builder {  get; }

    public Location Location { get; }

    public Building(string id, IUnit builder, StarSystem starSystem, Planet? planet)
    {
        Id = id;
        Builder = builder;
        Location = new Location(starSystem, planet);
    }
}
