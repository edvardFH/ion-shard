using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace IonShard.Domain.Units;

public abstract class AbstractUnit : IUnit
{
    public string Id { get; }
    public abstract string Type { get; }

    private ILocation _location;
    public virtual ILocation Location => _location;

    private Destination? _destination;
    public Destination? Destination => _destination;


    public AbstractUnit(string id, StarSystem system, Planet? planet)
    {
        Id = id;
        _location = new Location(system, planet);
    }

    async public Task Move(StarSystem system, Planet? planet)
    {
        int travelDuration = GetTravalDuration(system, planet);
        _destination = new Destination(system, planet, DateTime.Now.AddMilliseconds(travelDuration));

        bool unitLeavePlanet = this.Location.Planet is not null && this.Location.Planet != planet;
        bool systemChange = this.Location.System != system;
        bool unitEnterOnPlanet = this.Location.Planet != planet && planet is not null;

        
        if (unitLeavePlanet)
            _location = new Location(this.Location.System, null);

        if(systemChange)
        {
            await Task.Delay(60000);
            _location = new Location(system, null);
        }
     
        if(unitEnterOnPlanet)
        {
            await Task.Delay(15000);
            _location = new Location(system, planet);
        }
    }

    private int GetTravalDuration(StarSystem system, Planet? planet)
    {
        var result = 0;

        bool systemChange = this.Location.System != system;
        bool unitEnterOnPlanet = this.Location.Planet != planet && planet is not null;

        if(systemChange)
            result += 60000;

        if (unitEnterOnPlanet)
            result += 15000;

        return result;
    }
}
