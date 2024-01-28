using IonShard.Configuration.Units;

namespace IonShard.Configuration;

public interface IGameRulesService
{
    public IReadOnlyDictionary<string, IUnitConfiguration> Units { get; }
    public IReadOnlyDictionary<string, WeaponConfiguration> GetWeapons();
    public IReadOnlyDictionary<string, BuildingConfiguration> GetBuildings();
}
