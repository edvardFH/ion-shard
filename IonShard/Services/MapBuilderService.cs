using IonShard.Models.Space;
using Shard.Shared.Core;

namespace IonShard.Services;

public class MapBuilderService
{
    private readonly Universe _universe;

    public MapBuilderService(MapGenerator mapGenerator)
    {

        var systems = mapGenerator.Generate().Systems.ToList().ConvertAll(
            system => new StarSystem(system.Name, system.Planets.ToList().ConvertAll(
                planet => new Planet(planet.Name, planet.Size))
            )
        );
        _universe = new Universe(systems);
    }

    public List<StarSystem> GetAllSystems() => _universe.GetAllSystems();

    public StarSystem? GetSystem(string systemName) => _universe[systemName];

    public List<Planet>? GetPlanets(string systemName) => _universe[systemName]?.GetAllPlanets();

    public Planet? GetPlanet(string systemName, string planetName) => _universe[systemName]?[planetName];
}
