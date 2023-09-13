using IonShard.Models.Map;
using Shard.Shared.Core;

namespace IonShard.Services;

public class MapBuilderService
{
    public Universe Map { get; }

    public MapBuilderService(MapGenerator mapGenerator)
    {

        List<StarSystem> systems = mapGenerator.Generate().Systems.ToList().ConvertAll(
            system => new StarSystem(system.Name, system.Planets.ToList().ConvertAll(
                planet => new Planet(planet.Name, planet.Size))
            )
        );

        Map = new Universe(systems);
    }
}
