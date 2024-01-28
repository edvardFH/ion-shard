using IonShard.Domain.Buildings;
using IonShard.Domain.Units;
using System.ComponentModel.DataAnnotations;
using IonShard.Domain.Map.Resources;

namespace IonShard.Domain.Users;

public interface IUser
{
    [RegularExpression("^[a-zA-Z0-9_-]+$")]
    public string Id { get; }
    public string Pseudo { get; }
    public DateTime DateOfCreation { get; }
    public IReadOnlyDictionary<string, IUnit> Units { get; }
    public IReadOnlyDictionary<string, IBuilding> Buildings { get; }
    public IReadOnlyDictionary<IResource, int> ResourcesQuantity { get; }


    public void AddUnit(IUnit unit);
    public bool RemoveUnit(IUnit unit);
    public void AddBuilding(IBuilding building);
    public bool RemoveBuilding(IBuilding building);

    public void AddResource(IResource resource, int quantity);
    public bool HasResourcesFor(string unitType);
    public void UseResource(IResource resource, int quantity);
    public void UpdateResources(IReadOnlyDictionary<IResource, int> resources);
}
