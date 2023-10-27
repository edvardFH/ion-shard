using IonShard.Domain.Buildings;
using IonShard.Domain.Units;
using System.ComponentModel.DataAnnotations;

namespace IonShard.Domain.Users;

public class User: IUser
{
    [RegularExpression("^[a-zA-Z0-9_-]+$")]
    public string Id { get; }
    public string Pseudo { get; }
    public DateTime DateOfCreation { get; }
    public IDictionary<string, IUnit> Units { get; }
    public IDictionary<string, IBuilding> Buildings { get; }

    public User(string id, string pseudo, DateTime dateOfCreation)
    {
        Id = id;
        Pseudo = pseudo;
        DateOfCreation = dateOfCreation;
        Units = new Dictionary<string, IUnit>();
        Buildings = new Dictionary<string, IBuilding>();
    }
}
