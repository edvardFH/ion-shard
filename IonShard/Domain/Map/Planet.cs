using IonShard.Domain.Map.Resources;

namespace IonShard.Domain.Map;

public class Planet
{
    public string Name { get; }
    public int Size { get; }
    private readonly IDictionary<IResource, int> _resourceQuantity;
    public IReadOnlyDictionary<IResource, int> ResourcesQuantity
        => (IReadOnlyDictionary<IResource, int>)_resourceQuantity;

    public Planet(string name, int size, IReadOnlyDictionary<IResource, int> resourcesQuantity)
    {
        Name = name;
        Size = size;
        _resourceQuantity = (IDictionary<IResource, int>) resourcesQuantity;
    }

    public bool TakeOneResource(IResource resource)
    {
        if(!_resourceQuantity.ContainsKey(resource) || _resourceQuantity[resource] < 1)
            return false;

        _resourceQuantity[resource]--;
        return true;
    }
}
