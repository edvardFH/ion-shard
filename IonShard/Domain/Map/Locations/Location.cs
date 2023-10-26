namespace IonShard.Domain.Map.Locations;

public class Location : ILocation
{
    public StarSystem System { get; }
    public Planet? Planet { get; }


    public Location(StarSystem system, Planet? planet)
    {
        System = system;

        if (planet is not null && system[planet.Name] is null)
            throw new ArgumentException("This planet does not belong to the system.", planet.Name);

        Planet = planet;
    }
}
