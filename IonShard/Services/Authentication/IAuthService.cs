using IonShard.Configuration.Authentication;

namespace IonShard.Services.Authication;

public interface IAuthService
{
    public AuthUserEntity? Authenticate(string username, string password);
}