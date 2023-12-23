namespace IonShard.Contracts.DTO.Units;

public record UnitLocationDTO(string System, string? Planet, IReadOnlyDictionary<string, int>? ResourcesQuantity);
