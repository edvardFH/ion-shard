using IonShard.Domain.Map;

namespace IonShard.Domain.Units;

public class Unit
{
    public string Id { get; }
    public string Type => "scout";
    public Location Location { get; }

    public Unit(string id, StarSystem starSystem, Planet? planet)
    {
        Id = id;
        Location = new(starSystem, planet);
    }

}
