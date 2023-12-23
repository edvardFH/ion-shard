using IonShard.Domain.Map.Resources;

namespace IonShard.Contracts.RequestBodies;

public record UserPutRequestBody
    (
        string Id,
        string Pseudo,
        DateTime? DateOfCreation,
        IReadOnlyDictionary<string, int>? ResourcesQuantity
    );