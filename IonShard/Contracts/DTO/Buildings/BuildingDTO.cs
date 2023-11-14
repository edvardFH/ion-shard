namespace IonShard.Contracts.DTO.Buildings;

public record BuildingDTO(string Id, string Type, string System, string? Planet, bool IsBuilt, DateTime? EstimatedBuildTime);
