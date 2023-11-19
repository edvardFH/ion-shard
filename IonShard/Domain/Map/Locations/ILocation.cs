namespace IonShard.Domain.Map.Locations;

public interface ILocation
{
    public StarSystem System { get; }
    public Planet? Planet { get; }

    public bool IsPlanetLeft(Planet? newPlanet);
    public bool IsSystemChanged(StarSystem newStarSytem);
    public bool IsPlanetEntered(Planet? newPlanet);
}
