using IonShard.Domain.Map.Resources;

namespace IonShard.Contracts.RequestBodies;

public record CreateUserPutRequestBody(string Id, string Pseudo, IReadOnlyDictionary<string, int>? ResourcesQuantity);