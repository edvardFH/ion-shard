using IonShard.Domain.Units;

namespace IonShard.Domain.Users;

public class User
{
    public string Id { get; }
    public string Pseudo { get; }
    public DateTime DateOfCreation { get; }
    public IDictionary<string, Unit> Units { get; }

    public User(string id, string pseudo, DateTime dateOfCreation)
    {
        Id = id;
        Pseudo = pseudo;
        DateOfCreation = dateOfCreation;
        Units = new Dictionary<string, Unit>();
    }
}
