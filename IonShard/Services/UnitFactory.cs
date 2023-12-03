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
    public UnitFactory(IGameRulesService gameRulesService)
    {
        _weapons = gameRulesService.GetWeapons();
        _units = gameRulesService.GetUnits();
    }

    public IUnit GetNewUnit(IUser owner, StarSystem system, Planet? planet, string type)
    {
        if (!_units.ContainsKey(type))
            throw new ArgumentException(
                $"Incorrect unit type : {type} is not contained in configuration.");

        var unitStats = _units[type];

        return unitStats switch
        {
            CombatUnitConfiguration combatUnitStats =>
                new CombatUnit
                (
                    owner,
                    system,
                    planet,
                    type,
                    combatUnitStats.HealthPoints,
                    CreateWeapons(_weapons, combatUnitStats.Weapons),
                    combatUnitStats.CombatPriorities
                ),
            _ => CreatePeacefulUnit(owner, system, planet, type)
        };
    }

    public bool TypeExists(string type) => _units.ContainsKey(type);

    private IReadOnlyList<IWeapon> CreateWeapons(
        IReadOnlyDictionary<string, WeaponConfiguration> weapons,
        IReadOnlyDictionary<string, int> weaponsOnUnit)
    {
        return weaponsOnUnit
            .SelectMany(weaponQuantity =>
            {
                var weaponConfiguration = weapons[weaponQuantity.Key];
                var createdWeapons = new List<IWeapon>();
                for (var i = 0; i < weaponQuantity.Value; i++)
                {
                    createdWeapons.Add(
                        new Weapon
                        (
                            weaponQuantity.Key,
                            weaponConfiguration.Damage,
                            weaponConfiguration.Cooldown)
                        );
                }
                return createdWeapons;
            }).ToList();
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
