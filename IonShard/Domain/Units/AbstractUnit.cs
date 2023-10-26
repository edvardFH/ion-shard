using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using Shard.Shared.Core;

namespace IonShard.Domain.Units;

public abstract class AbstractUnit : IUnit
{
    private const int LeavePlanetManeuverDuration = 0;
    private const int ChangeSystemManeuverDuration = 60;
    private const int EnterPlanetManeuverDuration = 15;

    public string Id { get; }
    public abstract string Type { get; }

    private ILocation _location;
    public virtual ILocation Location => _location;

    private Destination? _destination;
    public Destination? Destination => _destination;

    private Task _travelTask = Task.CompletedTask;
    public Task TravelTask => _travelTask;

    public AbstractUnit(string id, StarSystem system, Planet? planet)
    {
        Id = id;
        _location = new Location(system, planet);
    }

    public void StartTravel(IClock clock, StarSystem destinationSystem, Planet? destinationPlanet)
    {
        var travelDuration = 0;

        if (_location.IsPlanetLeft(destinationPlanet))
            travelDuration += LeavePlanetManeuverDuration;

        if (_location.IsSystemChanged(destinationSystem))
            travelDuration += ChangeSystemManeuverDuration;

        if (_location.IsPlanetEntered(destinationPlanet))
            travelDuration += EnterPlanetManeuverDuration;

        _destination = new Destination(
            destinationSystem,
            destinationPlanet,
            clock.Now.Add(new TimeSpan(0, 0, travelDuration)));

        _travelTask = TravelAsync(clock);
    }

    private async Task TravelAsync(IClock clock)
    {
        if (_destination is null)
            return;


        if (_location.IsPlanetLeft(_destination.Planet))
            await clock.Delay(new TimeSpan(0, 0, LeavePlanetManeuverDuration));
        _location = new Location(_location.System, null);

        if (_location.IsSystemChanged(_destination.System))
        {
            await clock.Delay(new TimeSpan(0, 0, ChangeSystemManeuverDuration));
            _location = new Location(_destination.System, null);
        }

        if (_location.IsPlanetEntered(_destination.Planet))
        {
            await clock.Delay(new TimeSpan(0, 0, EnterPlanetManeuverDuration));
            _location = new Location(_destination.System, _destination.Planet);
        }
    }
}