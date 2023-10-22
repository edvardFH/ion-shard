using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;

namespace IonShard.Domain.Units;

public class Destination
{
    public ILocation Location { get; }
    public DateTime DateTime { get; }

    public Destination(StarSystem system, Planet? planet, DateTime dateTime)
    {
        Location = new Location(system,  planet);
        DateTime = dateTime;
    }
}
