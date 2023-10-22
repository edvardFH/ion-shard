using IonShard.Domain.Map;

namespace IonShard.Domain.Units;

public class Destination
{
    public Location Location { get; }
    public DateTime DateTime { get; }

    public Destination(StarSystem system, Planet? planet, DateTime dateTime)
    {
        Location = new Location(system,  planet);
        DateTime = dateTime;
    }
}
