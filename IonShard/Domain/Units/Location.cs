using IonShard.Domain.Map;

namespace IonShard.Domain.Units;

public class Location
{
    public StarSystem System { get; set; }
    public Planet? Planet { get; set; }


    public Location(StarSystem system, Planet? planet)
    {
        System = system;
        Planet = planet;
    }
}
