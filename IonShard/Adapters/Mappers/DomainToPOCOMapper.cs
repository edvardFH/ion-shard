using IonShard.Domain.Map;
using IonShard.Persistence.POCOs;

namespace IonShard.Adapters.Mappers;

public static class DomainToPOCOMapper
{
    public static StarSystemPOCO ToPOCO(this StarSystem starSystem)
    {
        return new StarSystemPOCO
        {
            Name = starSystem.Name,
            Planets = starSystem.Planets.Select(planet => planet.ToPOCO()).ToList()
        };
    }


    public static PlanetPOCO ToPOCO(this Planet planet)
    {
        return new PlanetPOCO
        {
            Name = planet.Name,
            Size = planet.Size,
            ResourcesQuantity = planet.ResourcesQuantity
                .ToDictionary
                (
                    keyValuePair => keyValuePair.Key.Name,
                    keyValuePair => keyValuePair.Value
                )
        };
    }
}

