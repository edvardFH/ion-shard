using System.Collections.ObjectModel;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;

namespace IonShard.UnitTests.Repository;

public class StarSystemTestProvider
{
    private static IDictionary<IResource, int>? _resources;
    
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
        if (_resources is not null)
            return new ReadOnlyDictionary<IResource, int>(_resources);
        
        _resources = new Dictionary<IResource, int>();
        _resources.Add(new Resource(ResourceName.Carbon), 3);
        _resources.Add(new Resource(ResourceName.Iron), 9);
        _resources.Add(new Resource(ResourceName.Gold), 7);
        _resources.Add(new Resource(ResourceName.Aluminium), 5);
        _resources.Add(new Resource(ResourceName.Titanium), 10);
        _resources.Add(new Resource(ResourceName.Water), 70);
        _resources.Add(new Resource(ResourceName.Oxygen), 20);
        return new ReadOnlyDictionary<IResource, int>(_resources);
    }
}