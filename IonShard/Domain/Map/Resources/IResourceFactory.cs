namespace IonShard.Domain.Map.Resources;

public interface IResourceFactory
{
    public IResource GetResource(string name);
    public IDictionary<IResource, int> TryParseToResourceQuantity(IReadOnlyDictionary<string, int> dictionary);
    public IDictionary<IResource, int> CompleteWithMissingResources(IReadOnlyDictionary<IResource, int> quantity);

    public bool DoesResourceExists(string name);
}
