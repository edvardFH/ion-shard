using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using MongoDB.Driver;
using Shard.Shared.Core;

namespace IonShard.Application;

public class MapFactory
{
    private readonly IResourceFactory _resourceFactory;

    public Universe Map { get; }

    public MapFactory(MapGenerator mapGenerator, IResourceFactory resourceFactory)
    {
        _resourceFactory = resourceFactory;
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


    private IReadOnlyDictionary<IResource, int> ResourceKindToResource(IReadOnlyDictionary<ResourceKind, int> resourceQuantity)
        => resourceQuantity
            .ToDictionary(
                resource => _resourceFactory.GetResource(resource.Key.ToString()),
                resource => resource.Value);
}
