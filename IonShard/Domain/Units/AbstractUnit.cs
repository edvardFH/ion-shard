using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using IonShard.Domain.Users;
using IonShard.Utils;
using Shard.Shared.Core;

namespace IonShard.Domain.Units;

public abstract class AbstractUnit : IUnit
{
    private const int LeavePlanetManeuverDuration = 0;
    private const int ChangeSystemManeuverDuration = 60;
    private const int EnterPlanetManeuverDuration = 15;

    public string Id { get; }
    public abstract string Type { get; }
    public IUser Owner { get; }
    public virtual ILocation Location { get; private set; }
    public IDestination? Destination { get; private set; }
    public Task TravelTask { get; private set; }


    public AbstractUnit(IUser owner, StarSystem system, Planet? planet)
    {
        Id = new Random().NextGuid().ToString();
        Owner = owner;
        Location = new Location(system, planet);
        TravelTask = Task.CompletedTask;
    }


    public void StartTravel(IClock clock, StarSystem destinationSystem, Planet? destinationPlanet)
    {
        var travelDuration = 0;

        if (Location.IsPlanetLeft(destinationPlanet))
            travelDuration += LeavePlanetManeuverDuration;

        if (Location.IsSystemChanged(destinationSystem))
            travelDuration += ChangeSystemManeuverDuration;

        if (Location.IsPlanetEntered(destinationPlanet))
            travelDuration += EnterPlanetManeuverDuration;

        Destination = new Destination(
            destinationSystem,
            destinationPlanet,
            clock.Now.Add(new TimeSpan(0, 0, travelDuration)));

        TravelTask = TravelAsync(clock);
    }

    private async Task TravelAsync(IClock clock)
    {
        if (Destination is null)
            return;


        if (Location.IsPlanetLeft(Destination.Planet))
        {
            await clock.Delay(new TimeSpan(0, 0, LeavePlanetManeuverDuration));
            Location = new Location(Location.System, null);
        }


        if (Location.IsSystemChanged(Destination.System))
        {
            await clock.Delay(new TimeSpan(0, 0, ChangeSystemManeuverDuration));
            Location = new Location(Destination.System, null);
        }

        if (Location.IsPlanetEntered(Destination.Planet))
        {
            await clock.Delay(new TimeSpan(0, 0, EnterPlanetManeuverDuration));
            Location = new Location(Destination.System, Destination.Planet);
        }
    }
}