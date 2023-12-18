namespace IonShard.Configuration.Wormholes;

public class WormholesConfigService : IWormholesService
{
    private const string SectionName = "Wormholes";

    public IReadOnlyDictionary<string, WormholeConfig> Wormholes { get; }

    public WormholesConfigService(IConfiguration configuration)
    {
        Wormholes = configuration
            .GetSection(SectionName)
            .GetChildren()
            .Select(server =>
            (
                server.Key,
                Value: server.Get<WormholeConfig>() ?? throw new ConfigurationFormatException(SectionName)
            ))
            .ToDictionary(server => server.Key, server => server.Value);
    }


    public WormholeConfig? this[string key] => Wormholes.ContainsKey(key)
        ? Wormholes[key]
        : null;
}
