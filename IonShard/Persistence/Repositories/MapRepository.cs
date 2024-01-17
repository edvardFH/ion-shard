using IonShard.Domain.Map;
using IonShard.Persistence.Loaders;

namespace IonShard.Persistence.Repositories;

public class MapRepository
{
    private Universe _universe;

    public MapRepository() => _universe = new Universe(new List<StarSystem>());


    public IReadOnlyList<StarSystem> Systems => _universe.Systems;

    public StarSystem? this[string systemName] => _universe[systemName];

    public Planet? this[string systemName, string planetName]
        => _universe[systemName]?[planetName];

    public void InitUniverse(Universe universe)
    {
        if (_universe.Systems.Count > 0)
            throw new InvalidOperationException("Universe already initilized, it should not be changed !");

        _universe = universe;
    }
}
