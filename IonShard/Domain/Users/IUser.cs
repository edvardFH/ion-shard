using IonShard.Domain.Buildings;
using IonShard.Domain.Units;
using System.ComponentModel.DataAnnotations;

namespace IonShard.Domain.Users;

public interface IUser
{
    [RegularExpression("^[a-zA-Z0-9_-]+$")]
    public string Id { get; }
    public string Pseudo { get; }
    public DateTime DateOfCreation { get; }
    public IReadOnlyDictionary<string, IUnit> Units { get; }
    public IReadOnlyDictionary<string, IBuilding> Buildings { get; }


    public void AddUnit(IUnit unit);
    public void AddBuilding(IBuilding building);
}
