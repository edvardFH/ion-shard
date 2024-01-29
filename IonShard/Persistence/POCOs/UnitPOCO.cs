namespace IonShard.Persistence.POCOs;

public class UnitPOCO
{
    public string Id { get; init; }
    public string Owner { get; init; }
    public string System { get; init; }
    public string? Planet { get; init; }
    public string Type { get; init; }
    public int HealthPoints { get; init; }
    public IReadOnlyDictionary<string, int>? LoadedResources { get; init; }
}
