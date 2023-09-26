using IonShard.Domain.Map;
using IonShard.Domain.Units;

namespace IonShard.DTO.Units;

public class UnitLocation
{
    public Unit Unit { get; }
    public StarSystem System { get; }
    public Planet? Planet { get; }
    public IReadOnlyDictionary<Resource, int>? RessourceQuantity { get; }

    public UnitLocation(Unit unit, StarSystem system, Planet? planet, IReadOnlyDictionary<Resource, int>? ressourceQuantity)
    {
        Unit = unit;
        System = system;
        Planet = planet;
        RessourceQuantity = ressourceQuantity;
    }
}
