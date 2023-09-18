using IonShard.Models.Map;

namespace IonShard.Models.Units;

public class UnitLocation
{
    public Unit Unit { get; }
    public StarSystem System { get; }
    public Planet? Planet { get; }
    public IReadOnlyDictionary<string, int>? RessourceQuantity { get; }
}
