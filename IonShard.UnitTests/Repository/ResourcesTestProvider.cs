using System.Collections.ObjectModel;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;

namespace IonShard.UnitTests.Repository;

public static class ResourcesTestProvider
{
    private static IReadOnlyDictionary<IResource, int>? _resources;

    public static IReadOnlyDictionary<IResource, int> GetResources()
    {
        if (_resources is not null) return _resources;
        var dictionary = new Dictionary<IResource, int>
        {
            { new Resource("Carbon", ResourceCategory.Solid, 10), 3 },
            { new Resource("Iron", ResourceCategory.Solid, 20), 9 },
            { new Resource("Gold", ResourceCategory.Solid, 40), 7 },
            { new Resource("Aluminium", ResourceCategory.Solid, 30), 5 },
            { new Resource("Titanium", ResourceCategory.Solid, 50), 10 },
            { new Resource("Water", ResourceCategory.Liquid, 10), 70 },
            { new Resource("Oxygen", ResourceCategory.Gaseous, 10), 20 }
        };

        _resources = new ReadOnlyDictionary<IResource, int>(dictionary);

        return _resources;
    }
}