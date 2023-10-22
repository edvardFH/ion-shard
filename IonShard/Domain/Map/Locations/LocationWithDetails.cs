namespace IonShard.Domain.Map.Locations;

public class LocationWithDetails : Location, ILocationWithDetails
{
    public IReadOnlyDictionary<Resource, int>? ResourcesQuantity { get; }

    public LocationWithDetails(StarSystem system, Planet? planet) : base(system, planet)
    {
        ResourcesQuantity = planet?.ResourcesQuantity;
    }
}
