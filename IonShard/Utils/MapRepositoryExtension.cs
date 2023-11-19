using IonShard.Persistence.Repositories;

namespace IonShard.Utils;

public static class MapRepositoryExtension
{
    public static string? getSystemNameOfAPlanet(this MapRepository repository, string planetName)
    {
        string? systemName = repository.Systems
            .Where(system => system.Planets.Any(planet => planet.Name == planetName))
            .Select(system => system.Name)
            .FirstOrDefault();
        return systemName;
    }
}