using IonShard.Models.Map;

namespace IonShard.Services;

public class MapRepository
{
    private readonly Universe _universe;

    public MapRepository(MapBuilderService mapBuilder)
    {
        _universe = mapBuilder.Map;
    }

    public IReadOnlyList<StarSystem> GetAllSystems() => _universe.Systems;

    public StarSystem? GetOneSystem(string systemName) => _universe[systemName];

    public IReadOnlyList<Planet>? GetAllPlanetsFromSystem(string systemName) => _universe[systemName]?.Planets;

    public Planet? GetOnePlanet(string systemName, string planetName) => _universe[systemName]?[planetName];
}
