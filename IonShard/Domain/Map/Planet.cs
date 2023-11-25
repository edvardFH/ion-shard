using IonShard.Domain.Map.Resources;

namespace IonShard.Domain.Map;

public class Planet
{
    public string Name { get; }
    public int Size { get; }
    public IReadOnlyDictionary<IResource, int> ResourcesQuantity { get; }

    public Planet(string name, int size, IReadOnlyDictionary<IResource, int> resourcesQuantity)
    {
        Name = name;
        Size = size;
        ResourcesQuantity = resourcesQuantity;
    }
}
