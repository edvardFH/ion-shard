using IonShard.Configuration;
using IonShard.Configuration.Authentication;

namespace IonShard.Services.Authication;

public class AuthService : IAuthService
{
    private const string AdminSection = "authUsers";
    private const string ServerSection = "Wormholes";
    private const string ShardNameStart = "shard-";

    private readonly IReadOnlyDictionary<string, AuthUserEntity> _users;

    public AuthService(IConfiguration configuration)
    {
        var users = configuration
                     .GetSection(AdminSection)
                     .Get<IDictionary<string, AuthUserEntity>>()
                 ?? throw new ConfigurationFormatException(AdminSection);

        GetServersFromConfig(configuration, ServerSection)
            .ToList()
            .ForEach(server => users.Add(
                server.Key,
                new AuthUserEntity(
                        server.Value.User,
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


    private IReadOnlyDictionary<string, ServerConfig> GetServersFromConfig(IConfiguration configuration, string serverSection) =>
        configuration
            .GetSection(serverSection)
            .GetChildren()
            .Select(server =>
            (
                server.Key,
                Value: server.Get<ServerConfig>() ?? throw new ConfigurationFormatException(serverSection)
            ))
            .ToDictionary(server => server.Key, server => server.Value);
}