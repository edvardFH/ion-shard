using IonShard.Domain.Buildings;
using IonShard.Domain.Units;
using System.ComponentModel.DataAnnotations;
using IonShard.Domain.Map;

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
    
    private readonly IDictionary<Resource, int> _resourcesQuantity;
    public IReadOnlyDictionary<Resource, int> ResourcesQuantity
        => (IReadOnlyDictionary<Resource, int>)_resourcesQuantity;


    public User(string id, string pseudo, DateTime dateOfCreation)
    {
        Id = id;
        Pseudo = pseudo;
        DateOfCreation = dateOfCreation;
        _units = new Dictionary<string, IUnit>();
        _buildings = new Dictionary<string, IBuilding>();
        _resourcesQuantity = new Dictionary<Resource, int>
        {
            { Resource.Carbon, 20 },
            { Resource.Iron, 10 },
            { Resource.Oxygen, 50 },
            { Resource.Water, 50 },
            { Resource.Aluminium, 0 },
            { Resource.Gold, 0 },
            { Resource.Titanium, 0 }
        };
    }


    public void AddUnit(IUnit unit) => _units.Add(unit.Id, unit);
    public void AddBuilding(IBuilding building) => _buildings.Add(building.Id, building);
    public void RemoveBuilding(string buildingId) => _buildings.Remove(buildingId);
}
