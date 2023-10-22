using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;

namespace IonShard.Domain.Units;

public class Destination
{
    private ILocation _location;
    public StarSystem System => _location.System;
    public Planet? Planet => _location.Planet;
    public DateTime EstimatedTimeOfArrival { get; }

    public Destination(StarSystem system, Planet? planet, DateTime dateTime)
    {
        _location = new Location(system,  planet);
        EstimatedTimeOfArrival = dateTime;
    }
}
