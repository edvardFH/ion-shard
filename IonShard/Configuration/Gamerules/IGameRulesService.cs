using IonShard.Configuration.Gamerules.Buildings;
using IonShard.Configuration.Gamerules.Resources;
using IonShard.Configuration.Gamerules.Units;
using IonShard.Configuration.Gamerules.Users;

namespace IonShard.Configuration.Gamerules;

public interface IGameRulesService
{
    public IReadOnlyDictionary<string, ResourceConfiguration> Resources { get; }
    public IReadOnlyDictionary<string, IUnitConfiguration> Units { get; }
    public IReadOnlyDictionary<string, WeaponConfiguration> Weapons { get; }
    public IReadOnlyDictionary<string, BuildingConfiguration> Buildings { get; }
    public UserConfiguration User {  get; }
}
