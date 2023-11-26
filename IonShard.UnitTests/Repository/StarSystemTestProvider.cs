using System.Collections.ObjectModel;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;

namespace IonShard.UnitTests.Repository;

public static class StarSystemTestProvider
{
    private static IReadOnlyDictionary<IResource, int>? _resources;

    public static List<StarSystem> ProvideSystems()
    {
        var starSystems = new List<StarSystem>();
        var solPlanets = new[]
        {
            new Planet("earth", 12742, GetResources()),
            new Planet("mars", 6779, GetResources())
        };
        var sol = new StarSystem("sol", solPlanets);
        var alphaCentauriPlanets = new[]
        {
            new Planet("proxima", 3540, GetResources()),
            new Planet("centauri-a", 89202, GetResources())
        };
        var alphaCentauri = new StarSystem("alpha-centauri", alphaCentauriPlanets);
        
        starSystems.Add(sol);
        starSystems.Add(alphaCentauri);
        
        return starSystems;
    }

    private static IReadOnlyDictionary<IResource, int> GetResources()
    {
        if (_resources is not null) return _resources;
        var dictionary = new Dictionary<IResource, int>
        {
            { new Resource(ResourceName.Carbon), 3 },
            { new Resource(ResourceName.Iron), 9 },
            { new Resource(ResourceName.Gold), 7 },
            { new Resource(ResourceName.Aluminium), 5 },
            { new Resource(ResourceName.Titanium), 10 },
            { new Resource(ResourceName.Water), 70 },
            { new Resource(ResourceName.Oxygen), 20 }
        };

        _resources = new ReadOnlyDictionary<IResource, int>(dictionary);

        return _resources;
    }
}