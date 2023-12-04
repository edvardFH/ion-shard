using IonShard.Configuration.Units;

namespace IonShard.Configuration;

public interface IGameRulesService
{
    public IReadOnlyDictionary<string, WeaponConfiguration> GetWeapons();
    public IReadOnlyDictionary<string, IUnitConfiguration> GetUnits();
    public IReadOnlyDictionary<string, BuildingConfiguration> GetBuildings();
}
