namespace IonShard.Contracts.DTO.Users;

public record UserDTO(string Id, string Pseudo, DateTime DateOfCreation, IReadOnlyDictionary<string, int> ResourcesQuantity);
