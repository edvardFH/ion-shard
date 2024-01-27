using IonShard.Domain.Map.Resources;

namespace IonShard.Domain.Map.Locations;

public interface ILocationWithDetails : ILocation
{
    public IReadOnlyDictionary<IResource, int>? ResourcesQuantity { get; }
}
