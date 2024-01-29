namespace IonShard.Persistence.POCOs;

using System.Collections.Generic;

public class PlanetPOCO
{
    public string Name { get; init; }
    public int Size { get; init; }
    public IReadOnlyDictionary<string, int> ResourcesQuantity { get; init; }
}
