using IonShard.Configuration;
using IonShard.Configuration.Authentication;

namespace IonShard.Services.Authication;

public class AuthService : IAuthService
{
    private readonly IReadOnlyDictionary<string, AuthUserEntity> _users;

    public AuthService(IConfiguration configuration)
    {
        const string adminSection = "authUsers";
        var users = configuration
                     .GetSection(adminSection)
                     .Get<IDictionary<string, AuthUserEntity>>()
                 ?? throw new ConfigurationFormatException(adminSection);


        const string serverSection = "Wormholes";

        GetServersFromConfig(configuration, serverSection)
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
        return _users.TryGetValue(username, out var user) && user.Password.Equals(password) ? user : null;
    }



    private IReadOnlyDictionary<string, AuthServerEntity> GetServersFromConfig(IConfiguration configuration, string serverSection) =>
        configuration
            .GetSection(serverSection)
            .GetChildren()
            .Select(server =>
            (
                server.Key,
                Value: server.Get<AuthServerEntity>() ?? throw new ConfigurationFormatException(serverSection)
            ))
            .ToDictionary(server => server.Key, server => server.Value);
}