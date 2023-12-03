using IonShard.Models.Map;
using Shard.Shared.Core;

namespace IonShard.Services;

public class MapBuilderService
{
    public Universe Map { get; }

    public MapBuilderService(MapGenerator mapGenerator)
    {

        IReadOnlyList<StarSystem> systems = mapGenerator.Generate()
            .Systems
            .ToList()
            .ConvertAll(system => SystemSpecificationToStarSystem(system));

        Map = new Universe(systems);
    }


    private StarSystem SystemSpecificationToStarSystem(SystemSpecification system) 
        => new(system.Name,
            system.Planets
            .ToList()
            .ConvertAll(planet => new Planet(planet.Name, planet.Size)));
}
