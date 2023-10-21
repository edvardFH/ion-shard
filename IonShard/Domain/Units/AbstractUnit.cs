using IonShard.Domain.Map;

namespace IonShard.Domain.Units;

public abstract class AbstractUnit : IUnit
{
    public string Id { get; }
    public abstract string Type { get; }
    public Location Location { get; }

    public AbstractUnit(string id, StarSystem starSystem, Planet? planet)
    {
        Id = id;
        Location = new(starSystem, planet);
    }
}
