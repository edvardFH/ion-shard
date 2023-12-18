namespace IonShard.Configuration.Wormholes;

public interface IWormholesService
{
    public IReadOnlyDictionary<string, WormholeConfig> Wormholes { get; }
    public WormholeConfig? this[string key] { get; }
}
