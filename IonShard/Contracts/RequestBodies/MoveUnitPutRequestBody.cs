namespace IonShard.Contracts.RequestBodies;

public record MoveUnitPutRequestBody(string Id, string System, string? Planet, string? type = null);