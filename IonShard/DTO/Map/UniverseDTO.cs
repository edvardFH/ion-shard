using IonShard.Domain.Map;

namespace IonShard.DTO.Map;

public class UniverseDTO
{
    private readonly IReadOnlyDictionary<string, StarSystemDTO> _starSystems;

    public IReadOnlyList<StarSystemDTO> Systems => _starSystems.Values.ToList();

    public StarSystemDTO? this[string name] => _starSystems.ContainsKey(name) 
        ? _starSystems[name]
        : null;


    public UniverseDTO(IReadOnlyList<StarSystemDTO> starSystems)
    {
        _starSystems = starSystems.ToDictionary(system => system.Name, system => system);
    }

    public UniverseDTO(IReadOnlyList<StarSystem> starSystems)
    {
        _starSystems = starSystems.ToList()
            .ConvertAll(system => new StarSystemDTO(system.Name, system.Planets))
            .ToDictionary(system => system.Name, system => system);
    }
}
