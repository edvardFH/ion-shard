namespace IonShard.Domain.Map.Locations;

public interface ILocation
{
    public StarSystem System { get; set; }
    public Planet? Planet { get; set; }
}
