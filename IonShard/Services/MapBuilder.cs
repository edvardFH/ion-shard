using IonShard.Domain.Map;
using Shard.Shared.Core;

namespace IonShard.Services;

public class MapBuilder
{
    public Universe Map { get; }

    public MapBuilder(MapGenerator mapGenerator)
    {

        IReadOnlyList<StarSystem> systems = mapGenerator
            .Generate()
            .Systems
            .ToList()
            .ConvertAll(system => SystemSpecificationToStarSystem(system));

        Map = new Universe(systems);
    }


    private StarSystem SystemSpecificationToStarSystem(SystemSpecification system) 
        => new(system.Name,
            system.Planets
            .ToList()
            .ConvertAll(planet => new Planet(planet.Name, planet.Size, ResourceKindToResource(planet.ResourceQuantity))));



    private IReadOnlyDictionary<Resource, int> ResourceKindToResource(IReadOnlyDictionary<ResourceKind, int> resourceQuantity)
        => resourceQuantity
            .ToDictionary(resource => (Resource)resource.Key, resource => resource.Value);
}

