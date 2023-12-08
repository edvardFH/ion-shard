using IonShard.Domain.Map.Resources;

namespace IonShard.Contracts.RequestBodies;

public record CreateUserPutRequestBody(string Id, string Pseudo, IReadOnlyDictionary<ResourceName, int>? ResourcesQuantity)
{
    public IReadOnlyDictionary<IResource, int>? GetParsedResources()
        => ResourcesQuantity?
            .ToDictionary(x => (IResource) new Resource(x.Key), x => x.Value)
            .AsReadOnly();
}