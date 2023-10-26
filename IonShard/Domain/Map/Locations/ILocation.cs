namespace IonShard.Domain.Map.Locations;

public interface ILocation
{
    public StarSystem System { get; }
    public Planet? Planet { get; }

    public bool IsPlanetLeft(ILocation newLocation);
    public bool DoesSystemChange(ILocation newLocation);
    public bool IsPlanetEntered(ILocation newLocation);
}
