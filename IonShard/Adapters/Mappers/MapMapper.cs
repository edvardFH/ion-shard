using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Persistence.POCOs;
using MongoDB.Driver;
using Shard.Shared.Core;

namespace IonShard.Adapters.Mappers;

public class MapMapper
{
    private readonly IResourceFactory _resourceFactory;

    public MapMapper(IResourceFactory resourceFactory)
    {
        _resourceFactory = resourceFactory;
    }



    public Universe SectorSpecificationToUniverse(SectorSpecification sectorSpecification)
        => new Universe(
                sectorSpecification.Systems
                .Select(SystemSpecificationToStarSystem)
                .ToList());


    public List<StarSystem> SystemPocoToUniverse(List<StarSystemPOCO> starSystemPocos)
       => starSystemPocos.Select(SystemPocoToStarSystem).ToList();



    private StarSystem SystemSpecificationToStarSystem(SystemSpecification systemSpecification)
        => new StarSystem(
                systemSpecification.Name,
                systemSpecification.Planets
                .Select(PlanetSpecificationToPlanet)
                .ToList());


    private Planet PlanetSpecificationToPlanet(PlanetSpecification planetSpecification)
    {
        return new Planet(
            planetSpecification.Name,
            planetSpecification.Size,
            ResourceKindToResource(planetSpecification.ResourceQuantity));
    }


    private IReadOnlyDictionary<IResource, int> ResourceKindToResource(IReadOnlyDictionary<ResourceKind, int> resourceQuantity)
        => resourceQuantity.ToDictionary(
                resource => _resourceFactory.GetResource(resource.Key.ToString()),
                resource => resource.Value);


    private StarSystem SystemPocoToStarSystem(StarSystemPOCO systemPoco)
    {
        var planets = systemPoco.Planets.Select(PlanetPocoToPlanet).ToList();
        return new StarSystem(systemPoco.Name, planets);
    }


    private Planet PlanetPocoToPlanet(PlanetPOCO planetPoco)
    {
        var resources = planetPoco.ResourcesQuantity
            .ToDictionary(
                keyValuePair => _resourceFactory.GetResource(keyValuePair.Key),
                keyValuePair => keyValuePair.Value);

        return new Planet(planetPoco.Name, planetPoco.Size, resources);
    }
}
