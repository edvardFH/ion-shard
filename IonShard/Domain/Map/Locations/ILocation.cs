namespace IonShard.Domain.Map.Locations;

public interface ILocation
{
    public StarSystem System { get; }
    public Planet? Planet { get; }
}
