using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Shard.Shared.Core;

namespace IonShard.Domain.Units;

public abstract class AbstractUnit : IUnit
{
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

    public void StartMove(IClock clock, StarSystem destinationSystem, Planet? destinationPlanet)
    {
        int travelDuration = GetTravalDuration(destinationSystem, destinationPlanet);
        _destination = new Destination(destinationSystem, destinationPlanet, clock.Now.AddMilliseconds(travelDuration)); // todo: use TimeStamp

        _travelTask = MoveAsync(clock);
    }

    private async Task MoveAsync(IClock clock)
    {
        if (_destination is null)
            return;

        bool unitLeavePlanet = _location.Planet is not null && _location.Planet != _destination.Planet;
        bool systemChange = _location.System != _destination.System;
        bool unitEnterOnPlanet = _location.Planet != _destination.Planet && _destination.Planet is not null;


        if (unitLeavePlanet)
            _location = new Location(_location.System, null);

        if (systemChange)
        {
            await clock.Delay(60000);
            _location = new Location(_destination.System, null);
        }

        if (unitEnterOnPlanet)
        {
            await clock.Delay(15000);
            _location = new Location(_destination.System, _destination.Planet);
        }
    }

    private int GetTravalDuration(StarSystem system, Planet? planet)
    {
        var result = 0;

        bool systemChange = _location.System != system;
        bool unitEnterOnPlanet = _location.Planet != planet && planet is not null;

        if (systemChange)
            result += 60000;

        if (unitEnterOnPlanet)
            result += 15000;

        return result;
    }

    public bool HasToLeavePlanet
        => this.Destination is not null && this.Location.Planet is not null && this.Destination.Planet is null;

    public bool HasToChangeSystem
        => this.Destination is not null && this.Location.System != this.Destination.System;

    public bool HasToEnterPlanet
        => this.Destination is not null && this.Location.Planet != this.Destination.Planet;
}
