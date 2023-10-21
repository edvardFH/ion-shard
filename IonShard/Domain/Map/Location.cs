namespace IonShard.Domain.Map;

public class Location
{
    public StarSystem System { get; set; }
    public Planet? Planet { get; set; }


    public Location(StarSystem system, Planet? planet)
    {
        System = system;

        if (planet is not null && system[planet.Name] is not null)
            throw new ArgumentException("This planet does not belong to the system.", planet.Name);

        Planet = planet;
    }
}
