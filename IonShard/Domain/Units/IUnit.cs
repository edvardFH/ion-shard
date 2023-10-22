using IonShard.Domain.Map.Locations;

namespace IonShard.Domain.Units;

public interface IUnit
{
    public string Id { get; }
    public string Type { get; }
    public ILocation Location { get; }
    // public Destination? Destination { get; }
}
