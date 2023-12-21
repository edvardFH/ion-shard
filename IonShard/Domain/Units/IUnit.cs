using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Users;
using Shard.Shared.Core;

namespace IonShard.Domain.Units;

public interface IUnit
{
    public string Id { get; }
    public string Type { get; }
    public IUser Owner { get; }
    public ILocation Location { get; }
    public IDestination? Destination { get; }
    public IReadOnlyDictionary<IResource, int> ResourceCost { get; }
    public int HealthPoints { get; }

    public Task TravelTask { get; }
    public void StartTravel(IClock clock, StarSystem starSystem, Planet? planet);
    public bool TryRequestTravelStop();
}
