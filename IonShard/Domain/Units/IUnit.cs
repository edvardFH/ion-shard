using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using Shard.Shared.Core;

namespace IonShard.Domain.Units;

public interface IUnit
{
    public string Id { get; }
    public string Type { get; }
    public ILocation Location { get; }
    public Destination? Destination { get; }

    public Task TravelTask { get; }
    public void Move(IClock clock, StarSystem starSystem, Planet? planet);
}
