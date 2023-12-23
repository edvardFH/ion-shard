using IonShard.Domain.Map.Resources;
using System.Text.Json.Serialization;

namespace IonShard.Contracts.DTO.Buildings;

public record BuildingDTO
{
    public string Id { get; }
    public string Type { get; }
    public string System { get; }
    public string? Planet { get; }
    public bool IsBuilt { get; }
    public DateTime? EstimatedBuildTime { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ResourceCategory { get; }

    public BuildingDTO(
        string id,
        string type,
        string system,
        string? planet,
        bool isBuilt,
        DateTime? estimatedBuildTime,
        ResourceCategory? resourceCategory)
    {
        Id = id;
        Type = type;
        System = system;
        Planet = planet;
        IsBuilt = isBuilt;
        EstimatedBuildTime = estimatedBuildTime;
        ResourceCategory = resourceCategory?.ToString().ToLower();
    }
}
