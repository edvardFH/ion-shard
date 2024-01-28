using IonShard.Domain.Buildings;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units;

namespace IonShard.Domain.Map;

public class Planet
{
    public string Name { get; }
    public int Size { get; }
    private readonly IDictionary<IResource, int> _resourceQuantity;
    public IReadOnlyDictionary<IResource, int> ResourcesQuantity
        => _resourceQuantity.AsReadOnly();

    public IList<IUnit> Units { get; }
    public IList<IBuilding> Buildings { get; }

    public Planet(string name, int size, IReadOnlyDictionary<IResource, int> resourcesQuantity)
    {
        Name = name;
        Size = size;
        _resourceQuantity =  new Dictionary<IResource, int>(resourcesQuantity);
        Units = new List<IUnit>();
        Buildings = new List<IBuilding>();
    }

    public bool TakeOneResource(IResource resource)
    {
        if(!_resourceQuantity.ContainsKey(resource) || _resourceQuantity[resource] < 1)
            return false;

        _resourceQuantity[resource]--;
        return true;
    }
}
