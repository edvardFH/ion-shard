namespace IonShard.Models.Users;

public class User
{
    public string Id { get; }
    public string Pseudo { get; }
    public DateTime DateOfCreation { get; }

    public User(string id, string pseudo, DateTime dateOfCreation)
    {
        Id = id;
        Pseudo = pseudo;
        DateOfCreation = dateOfCreation;
    }
}
