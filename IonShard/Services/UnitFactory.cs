using IonShard.Configuration;
using IonShard.Configuration.Units;
using IonShard.Domain.Map;
using IonShard.Domain.Units;
using IonShard.Domain.Units.Builder;
using IonShard.Domain.Units.Combat;
using IonShard.Domain.Units.Scout;
using IonShard.Domain.Users;
using IonShard.Utils;

namespace IonShard.Services;

public class UnitFactory
{
    private IReadOnlyDictionary<string, WeaponConfiguration> _weapons;
    private IReadOnlyDictionary<string, IUnitConfiguration> _units;
    public UnitFactory(GameRulesConfigurationService gameRulesConfigurationService)
    {
        _weapons = gameRulesConfigurationService.GetWeapons();
        _units = gameRulesConfigurationService.GetUnits();
    }

    public IUnit GetNewUnit(IUser owner, StarSystem system, Planet? planet, string type)
    {
        if (!_units.ContainsKey(type))
            throw new ArgumentException(
                $"Incorrect unit type : {type} is not contained in configuration.");

        var unitStats = _units[type];
        var unit = unitStats switch
        {
            CombatUnitConfiguration combatUnitStats => new CombatUnit(
                owner,
                system,
                planet,
                type,
                combatUnitStats.HealthPoints,
                new List<IWeapon>(),
                new List<string>()),
            _ => CreatePeacefulUnit(owner, system, planet, type)
        };

        return null;
    }

    private IUnit CreatePeacefulUnit(
        IUser owner,
        StarSystem starSystem,
        Planet? planet,
        string type)
    {
        var unitStats = _units[type];
        return type switch
        {
            "Scout" => new ScoutUnit(owner, starSystem, planet),
            "Builder" => new BuilderUnit(owner, starSystem, planet),
            _ => throw new ArgumentException($"Incorrect unit type : {type} is unknown.")
        };
    }
}
