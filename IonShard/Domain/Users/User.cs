using IonShard.Domain.Buildings;
using IonShard.Domain.Units;
using System.ComponentModel.DataAnnotations;
using IonShard.Domain.Map.Resources;

namespace IonShard.Domain.Users;

public class User : IUser
{
    [RegularExpression("^[a-zA-Z0-9_-]+$")]
    public string Id { get; }
    public string Pseudo { get; }
    public DateTime DateOfCreation { get; }

    private readonly IDictionary<string, IUnit> _units;
    public IReadOnlyDictionary<string, IUnit> Units
        => (IReadOnlyDictionary<string, IUnit>)_units;

    private readonly IDictionary<string, IBuilding> _buildings;
    public IReadOnlyDictionary<string, IBuilding> Buildings
        => (IReadOnlyDictionary<string, IBuilding>)_buildings;

    private readonly IDictionary<IResource, int> _resourcesQuantity;
    public IReadOnlyDictionary<IResource, int> ResourcesQuantity
        => (IReadOnlyDictionary<IResource, int>)_resourcesQuantity;


    public User(string id, string pseudo, DateTime dateOfCreation)
    {
        Id = id;
        Pseudo = pseudo;
        DateOfCreation = dateOfCreation;
        _units = new Dictionary<string, IUnit>();
        _buildings = new Dictionary<string, IBuilding>();
        _resourcesQuantity = new Dictionary<IResource, int>
        {
            { new Resource(ResourceName.Carbon), 20 },
            { new Resource(ResourceName.Iron), 10 },
            { new Resource(ResourceName.Oxygen), 50 },
            { new Resource(ResourceName.Water), 50 },
            { new Resource(ResourceName.Aluminium), 0 },
            { new Resource(ResourceName.Gold), 0 },
            { new Resource(ResourceName.Titanium), 0 }
        };
    }


    public void AddUnit(IUnit unit) => _units.Add(unit.Id, unit);

    public void AddBuilding(IBuilding building) => _buildings.Add(building.Id, building);
    public void RemoveBuilding(string buildingId) => _buildings.Remove(buildingId);

    public void AddOneResource(IResource resource)
    {
        if (_resourcesQuantity.ContainsKey(resource))
            _resourcesQuantity[resource]++;
        else
            _resourcesQuantity.Add(resource, 1);
    }
}
