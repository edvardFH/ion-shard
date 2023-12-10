using IonShard.Configuration.Auth;

namespace IonShard.Services.Auth;

public interface IAuthService
{
    public AuthUserEntity? Authenticate(string username, string password);
}