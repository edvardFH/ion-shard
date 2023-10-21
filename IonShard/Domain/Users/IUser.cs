using IonShard.Domain.Units;
using System.ComponentModel.DataAnnotations;

namespace IonShard.Domain.Users;

public interface IUser
{
    [RegularExpression("^[a-zA-Z0-9_-]+$")]
    public string Id { get; }
    public string Pseudo { get; }
    public DateTime DateOfCreation { get; }
    public IDictionary<string, IUnit> Units { get; }
}
