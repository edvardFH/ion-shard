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
    private CancellationTokenSource? _cancellationTokenSource;


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


        _cancellationTokenSource = new CancellationTokenSource();

        TravelTask = TravelAsync(clock, _cancellationTokenSource.Token);
    }


    private async Task TravelAsync(IClock clock, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (Destination is null)
            throw new InvalidOperationException("Unit destination is null. Travel is impossible.");


        if (Location.IsPlanetLeft(Destination.Planet))
        {
            await clock.Delay(
                new TimeSpan(0, 0, LeavePlanetManeuverDuration),
                cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            Location = new Location(Location.System, null);
        }


        if (Location.IsSystemChanged(Destination.System))
        {
            await clock.Delay(
                new TimeSpan(0, 0, ChangeSystemManeuverDuration),
                cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            Location = new Location(Destination.System, null);
        }

        if (Location.IsPlanetEntered(Destination.Planet))
        {
            await clock.Delay(
                new TimeSpan(0, 0, EnterPlanetManeuverDuration),
                cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            Location = new Location(Destination.System, Destination.Planet);
        }
    }


    public bool TryRequestTravelStop()
    {
        var cancellationSuccessfullyRequested = false;

        if (TravelTask.Status == TaskStatus.Running && _cancellationTokenSource is not null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
            cancellationSuccessfullyRequested = true;
        }

        return cancellationSuccessfullyRequested;
    }
}