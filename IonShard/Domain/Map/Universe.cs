namespace IonShard.Domain.Map;

public class Universe
{
    private readonly IReadOnlyDictionary<string, StarSystem> _starSystems;

    public IReadOnlyList<StarSystem> Systems => _starSystems.Values.ToList();

    public StarSystem? this[string name] => _starSystems.ContainsKey(name)
        ? _starSystems[name]
        : null;


    public Universe(IReadOnlyList<StarSystem> starSystems)
    {
        _starSystems = starSystems.ToDictionary(system => system.Name, system => system);
    }
}
