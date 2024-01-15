namespace IonShard.Persistence.POCOs;

using System.Collections.Generic;

public class PlanetPOCO
{
    public string Name { get; set; }
    public int Size { get; set; }
    public Dictionary<string, int> ResourcesQuantity { get; set; }
}
