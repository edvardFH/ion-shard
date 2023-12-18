using IonShard.Configuration;
using IonShard.Configuration.Authentication;
using IonShard.Configuration.Wormholes;

namespace IonShard.Services.Authication;

public class AuthService : IAuthService
{
    private const string AdminSection = "authUsers";
    private const string ShardNameStart = "shard-";

    private readonly IReadOnlyDictionary<string, AuthUserEntity> _users;

    public AuthService(IConfiguration configuration, IWormholesService wormholesService)
    {
        var users = configuration
                     .GetSection(AdminSection)
                     .Get<IDictionary<string, AuthUserEntity>>()
                 ?? throw new ConfigurationFormatException(AdminSection);

        wormholesService.Wormholes
            .ToList()
            .ForEach(server => users.Add(
                server.Key,
                new AuthUserEntity(
                        server.Key,
                        server.Value.SharedPassword,
                        "Shard")));


        _users = users.AsReadOnly();
    }

    public AuthUserEntity? Authenticate(string username, string password)
    {
        var formattedUserName = IsShard(username)
            ? username.Substring(ShardNameStart.Length)
            : username;

        return _users.TryGetValue(formattedUserName, out var user) && user.Password.Equals(password) ? user : null;
    }


    private bool IsShard(string username) => username.StartsWith(ShardNameStart);
}