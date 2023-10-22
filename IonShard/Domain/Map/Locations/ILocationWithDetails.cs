namespace IonShard.Domain.Map.Locations;

public interface ILocationWithDetails : ILocation
{
    public IReadOnlyDictionary<Resource, int>? ResourcesQuantity { get; }
}
