using IonShard.Domain.Map;
using IonShard.DTO.Map;

namespace IonShard.DTO.Units;

public class UnitLocationDTO
{
    public string System { get; }
    public string? Planet { get; }
    public IReadOnlyDictionary<string, int>? RessourceQuantity { get; }

    public UnitLocationDTO(string system, string? planet, IReadOnlyDictionary<string, int>? ressourceQuantity)
    {
        System = system;
        Planet = planet;
        RessourceQuantity = ressourceQuantity;
    }
}
