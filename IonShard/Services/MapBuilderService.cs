using IonShard.Models.Space;
using Shard.Shared.Core;

namespace IonShard.Services;

public class MapBuilderService
{
    private readonly Universe _universe;

    public MapBuilderService(MapGenerator mapGenerator)
    {

        List<StarSystem> systems = mapGenerator.Generate().Systems.ToList().ConvertAll(
            system => new StarSystem(system.Name, system.Planets.ToList().ConvertAll(
                planet => new Planet(planet.Name, planet.Size))
            )
        );

        _universe = new Universe(systems);
    }


    public IReadOnlyList<StarSystem> GetAllSystems() => _universe.Systems;

    public StarSystem? GetOneSystem(string systemName) => _universe[systemName];

    public IReadOnlyList<Planet>? GetAllPlanetsFromSystem(string systemName) => _universe[systemName]?.Planets;

    public Planet? GetOnePlanet(string systemName, string planetName) => _universe[systemName]?[planetName];
}
