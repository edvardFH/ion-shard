using IonShard.Domain.Map;
using IonShard.DTO.Map;

namespace IonShard.DTO.Units;

public class UnitLocationDTO
{
    public UnitDTO Unit { get; }
    public StarSystemDTO System { get; }
    public PlanetDTO? Planet { get; }
    public IReadOnlyDictionary<Resource, int>? RessourceQuantity { get; }

    public UnitLocationDTO(UnitDTO unit, StarSystemDTO system, PlanetDTO? planet, IReadOnlyDictionary<Resource, int>? ressourceQuantity)
    {
        Unit = unit;
        System = system;
        Planet = planet;
        RessourceQuantity = ressourceQuantity;
    }
}
