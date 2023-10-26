namespace IonShard.Domain.Map.Locations;

public class Location : ILocation
{
    public StarSystem System { get; }
    public Planet? Planet { get; }


    public Location(StarSystem system, Planet? planet)
    {
        System = system;

        if (planet is not null && system[planet.Name] is null)
            throw new ArgumentException("This planet does not belong to the system.", planet.Name);

        Planet = planet;
    }

    public static bool operator ==(Location leftLocation, Location rightLocation) 
        => leftLocation.System == rightLocation.System && leftLocation.Planet == rightLocation.Planet;
    public static bool operator !=(Location leftLocation, Location rightLocation)
        => leftLocation.System != rightLocation.System || leftLocation.Planet == rightLocation.Planet;

    public bool IsPlanetLeft(ILocation newLocation)
        => this.Planet is not null && this.Planet != newLocation.Planet;

    public bool DoesSystemChange(ILocation newLocation)
        => this.System != newLocation.System;

    public bool IsPlanetEntered(ILocation newLocation)
        => this.Planet != newLocation.Planet;
}
