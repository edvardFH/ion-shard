using IonShard.Configuration;
using IonShard.Configuration.Authentication;

namespace IonShard.Services.Authication;

public class AuthService : IAuthService
{
    private readonly IReadOnlyDictionary<string, AuthUserEntity> _users;

    public AuthService(IConfiguration configuration)
    {
        const string key = "authUsers";
        _users = configuration
                     .GetSection(key)
                     .Get<IReadOnlyDictionary<string, AuthUserEntity>>() 
                 ?? throw new ConfigurationFormatException(key);
    }

    public AuthUserEntity? Authenticate(string username, string password)
    {
        return _users.TryGetValue(username, out var user) && user.Password.Equals(password) ? user : null;
    }
}