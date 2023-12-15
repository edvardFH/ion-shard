using IonShard.Configuration.Gamerules.Units;

namespace IonShard.Configuration.Gamerules;

public interface IGameRulesService
{
    public IReadOnlyDictionary<string, IUnitConfiguration> Units { get; }
    public IReadOnlyDictionary<string, WeaponConfiguration> GetWeapons();
    public IReadOnlyDictionary<string, BuildingConfiguration> GetBuildings();
}
