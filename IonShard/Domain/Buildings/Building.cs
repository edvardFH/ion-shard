using IonShard.Domain.Map;
using IonShard.Domain.Units;

namespace IonShard.Domain.Buildings;

public class Building
{
    public string Id { get; }

    public string Type => "mine";

    public Unit Unit {  get; }

    public Location Location { get; }

    public Building(string id, Unit unit, StarSystem starSystem, Planet? planet)
    {
        Id = id;
        Unit = unit;
        Location = new Location(starSystem, planet);
    }
}
