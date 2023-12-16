namespace IonShard.Contracts.RequestBodies;

public record MoveUnitPutRequestBody(
    string Id, 
    string System, 
    string? Planet, 
    string? DestinationSystem, 
    string? DestinationPlanet, 
    string? Type = null,
    int Health = 0,
    IReadOnlyDictionary<string, int>? ResourcesQuantity = null);