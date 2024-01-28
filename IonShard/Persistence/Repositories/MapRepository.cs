using IonShard.Application;
using IonShard.Domain.Map;

namespace IonShard.Persistence.Repositories;

public class MapRepository
{
    private readonly Universe _universe;

    public MapRepository(MapBuilder mapBuilder) => _universe = mapBuilder.Map;


    public IReadOnlyList<StarSystem> Systems => _universe.Systems;

    public StarSystem? this[string systemName] => _universe[systemName];

    public Planet? this[string systemName, string planetName]
        => _universe[systemName]?[planetName];
}
