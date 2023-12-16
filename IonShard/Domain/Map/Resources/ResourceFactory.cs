using IonShard.Configuration;
using IonShard.Configuration.Gamerules;
using IonShard.Utils;
using System.Xml.Linq;

namespace IonShard.Domain.Map.Resources;

public class ResourceFactory : IResourceFactory
{
    private readonly IReadOnlyDictionary<string, IResource> _resources;


    public ResourceFactory(IGameRulesService gameRulesService)
    {
        _resources = gameRulesService.Resources
            .ToList()
            .Select(resource =>
            {
                var categoryIsValid = Enum.TryParse<ResourceCategory>(resource.Value.Category, out var category);

                if (categoryIsValid)
                    return new Resource
                       (
                            resource.Key,
                            category,
                            resource.Value.Rarity
                       );

                throw new ConfigurationFormatException("ResourceCategory");
            })
            .ToDictionary(resource => resource.Name, resource => (IResource)resource);
    }


    public IResource GetResource(string name)
    {
        if (!DoesResourceExists(name))
            throw new ArgumentException(
                $"Incorrect resource name : {name} is not contained in game rules.");

        return _resources[name];
    }


    public IDictionary<IResource, int> TryParseToResourceQuantity(IReadOnlyDictionary<string, int> quantity)
    {
        return quantity.ToList()
            .Select(resourceQuantity =>
                (
                    Resource: GetResource(resourceQuantity.Key.UppercaseFirstWord()),
                    Quantity: resourceQuantity.Value)
                )
            .ToDictionary
                (
                    resourceQuantity => resourceQuantity.Resource,
                    resourceQuantity => resourceQuantity.Quantity
                );
    }


    public bool DoesResourceExists(string name) => _resources.ContainsKey(name);
}
