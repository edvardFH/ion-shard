using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Users;
using IonShard.Utils;
using Shard.Shared.Core;

namespace IonShard.Domain.Units;

public abstract class Unit : IUnit
{
    private const int LeavePlanetManeuverDuration = 0;
    private const int ChangeSystemManeuverDuration = 60;
    private const int EnterPlanetManeuverDuration = 15;

    public string Id { get; }
    public string Type { get; }
    public IUser Owner { get; }
    public virtual ILocation Location { get; private set; }
    public IDestination? Destination { get; private set; }
    public IReadOnlyDictionary<Resource, int> ResourceCost {  get; }
    public Task TravelTask { get; private set; }

    private CancellationTokenSource? _cancellationTokenSource;


    public Unit
        (
            IUser owner,
            StarSystem system,
            Planet? planet,
            string type,
            IReadOnlyDictionary<Resource, int> resourceCost
        )
    {
        Id = new Random().NextGuid().ToString();
        Owner = owner;
        Location = new Location(system, planet);
        TravelTask = Task.CompletedTask;
        Type = type;
        ResourceCost = resourceCost;
    }


    public virtual void StartTravel(IClock clock, StarSystem destinationSystem, Planet? destinationPlanet)
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
            clock.Now.Add(TimeSpan.FromSeconds(travelDuration)));


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

            Location.Planet?.Units.Remove(this);
            Location = new Location(Location.System, null);
            Location.System.Units.Add(this);
        }


        if (Location.IsSystemChanged(Destination.System))
        {
            await clock.Delay(
                new TimeSpan(0, 0, ChangeSystemManeuverDuration),
                cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            Location.System.Units.Remove(this);
            Location = new Location(Destination.System, null);
            Location.System.Units.Add(this);
        }

        if (Location.IsPlanetEntered(Destination.Planet))
        {
            await clock.Delay(
                new TimeSpan(0, 0, EnterPlanetManeuverDuration),
                cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            Location.System.Units.Remove(this);
            Location = new Location(Destination.System, Destination.Planet);
            Location.Planet?.Units.Add(this);
        }

        Destination = null;
        _cancellationTokenSource = null;
    }


    public bool TryRequestTravelStop()
    {
        var cancellationSuccessfullyRequested = false;

        if (TravelTask.Status is TaskStatus.Running or TaskStatus.WaitingForActivation && _cancellationTokenSource is not null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
            cancellationSuccessfullyRequested = true;
        }

        return cancellationSuccessfullyRequested;
    }
}