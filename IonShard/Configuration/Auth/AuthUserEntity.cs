namespace IonShard.Configuration.Auth;

public class AuthUserEntity
{
    public string Username { get; }
    public string Password { get; }
    public string Role { get; }

    public AuthUserEntity(string username, string password, string role)
    {
        Username = username;
        Password = password;
        Role = role;
    }
}