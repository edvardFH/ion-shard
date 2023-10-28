using IonShard.Domain.Map;

namespace IonShard.Domain.Map.Locations;

public class Destination : Location, IDestination
{
    public DateTime EstimatedTimeOfArrival { get; }

    public Destination(StarSystem system, Planet? planet, DateTime dateTime) : base(system, planet)
    {
        EstimatedTimeOfArrival = dateTime;
    }
}
