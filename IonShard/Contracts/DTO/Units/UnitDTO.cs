namespace IonShard.Contracts.DTO.Units;

public record UnitDTO(
    string Id,
    string Type,
    string System,
    string? Planet,
    string? DestinationSystem,
    string? DestinationPlanet,
    string? EstimatedTimeOfArrival,
    int Health);
