namespace IonShard.Configuration.Wormholes;

public interface IWormholesConfigService
{
    public IReadOnlyDictionary<string, WormholeConfig> Wormholes { get; }
    public WormholeConfig? this[string key] { get; }

    public WormholeConfig? GetByUri(string uri);
}
