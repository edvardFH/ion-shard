using IonShard.Domain.Map;

namespace IonShard.DTO.Map;

public class StarSystemDTO
{
    private readonly IReadOnlyDictionary<string, PlanetDTO> _planets;


    public string Name { get; }

    public IReadOnlyList<PlanetDTO> Planets => _planets.Values.ToList();

    public PlanetDTO? this[string name] => _planets.ContainsKey(name) ? _planets[name] : null;


    public StarSystemDTO(string name, IReadOnlyList<PlanetDTO> planets)
    {
        Name = name;
        _planets = planets.ToDictionary(planet => planet.Name, planet => planet);
    }
    
    public StarSystemDTO(string name, IReadOnlyList<Planet> planets)
    {
        Name = name;
        _planets = planets.ToList()
            .ConvertAll(planet => new PlanetDTO(planet.Name, planet.Size))
            .ToDictionary(planet => planet.Name, planet => planet);
    }
}
