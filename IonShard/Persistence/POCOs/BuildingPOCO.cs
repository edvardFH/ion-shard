namespace IonShard.Persistence.POCOs;

public class BuildingPOCO
{
    public string Id { get; init; }
    public string Type { get; init; }
    public string Builder { get; init; }
    public string System { get; init; }
    public string Planet { get; init; }
    public bool IsBuilt { get; init; }
    public DateTime? EstimatedBuildTime { get; init; }
    public string? ResourceCategory { get; init; }
}
