using IonShard.Domain.Units;

namespace IonShard.Domain.Map;

public class StarSystem
{
    private readonly IReadOnlyDictionary<string, Planet> _planets;

    public string Name { get; }

    public IReadOnlyList<Planet> Planets => _planets.Values.ToList();

    public Planet? this[string name] => _planets.ContainsKey(name) ? _planets[name] : null;
    public IList<IUnit> Units { get; }


    public StarSystem(string name, IReadOnlyList<Planet> planets)
    {
        Name = name;
        _planets = planets.ToDictionary(planet => planet.Name, planet => planet);
        Units = new List<IUnit>();
    }
}
