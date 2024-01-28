using IonShard.Configuration.Authentication;

namespace IonShard.Application.Authentication;

public interface IAuthService
{
    public AuthUserEntity? Authenticate(string username, string password);
}