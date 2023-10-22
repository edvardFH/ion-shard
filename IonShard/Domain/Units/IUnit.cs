using IonShard.Domain.Map;

namespace IonShard.Domain.Units;

public interface IUnit
{
    public string Id { get; }
    public string Type { get; }
    public Location Location { get; }
    // public Destination? Destination { get; }
}
