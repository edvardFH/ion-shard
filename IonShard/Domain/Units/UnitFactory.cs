using IonShard.Configuration;
using IonShard.Configuration.Gamerules;
using IonShard.Configuration.Gamerules.Units;
using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units.Builder;
using IonShard.Domain.Units.Combat;
using IonShard.Domain.Units.Scout;
using IonShard.Domain.Users;
using IonShard.Utils;
using Shard.Shared.Core;

namespace IonShard.Domain.Units;

public class UnitFactory : IUnitFactory
{
    private readonly IReadOnlyDictionary<string, WeaponConfiguration> _weapons;
    private readonly IReadOnlyDictionary<string, IUnitConfiguration> _units;
    private readonly IClock _clock;


    public UnitFactory(IGameRulesService gameRulesService, IClock clock)
    {
        _weapons = gameRulesService.Weapons;
        _units = gameRulesService.Units;
        _clock = clock;
    }


    public IUnit CreateUnitWithId
        (
            string id,
            IUser owner,
            StarSystem system,
            Planet? planet,
            string type,
            IBuildingFactory? buildingFactory
        )
    {
        var formattedType = type.UppercaseFirstWord();

        if (!DoesTypeExist(formattedType))
            throw new ArgumentException(
                $"Incorrect unit type : {formattedType} is not contained in game rules.");

        var unitConfig = _units[formattedType];

        var newUnit = unitConfig switch
        {
            CombatUnitConfiguration combatUnitStats =>
                new CombatUnit
                (
                    id,
                    owner,
                    system,
                    planet,
                    CreateResourceCost(unitConfig),
                    formattedType,
                    combatUnitStats.HealthPoints,
                    CreateWeapons(_weapons, combatUnitStats.Weapons),
                    combatUnitStats.CombatPriorities,
                    combatUnitStats.DamageReductionMultipliers,
                    _clock
                ),
            _ =>
                CreatePeacefulUnit
                (
                    id,
                    owner,
                    system,
                    planet,
                    CreateResourceCost(unitConfig),
                    formattedType,
                    buildingFactory
                )
        };

        owner.AddUnit(newUnit);

        if (planet is null)
            system.Units.Add(newUnit);
        else
            planet.Units.Add(newUnit);

        return newUnit;
    }

    public IUnit CreateUnit
        (
            IUser owner,
            StarSystem system,
            Planet? planet,
            string type,
            IBuildingFactory? buildingFactory
        )
    {
        return CreateUnitWithId
            (
                new Random().NextGuid().ToString(),
                owner,
                system,
                planet,
                type,
                buildingFactory
            );
    }


    public bool DoesTypeExist(string type) =>
    _units.ContainsKey(type.UppercaseFirstWord());


    private IReadOnlyDictionary<Resource, int> CreateResourceCost(IUnitConfiguration unitConfiguration)
    {
        return unitConfiguration.ResourceCost
            .Select(resourceCost =>
            {
                if (!Enum.TryParse(resourceCost.Key, out ResourceName resource))
                    throw new ConfigurationFormatException(resourceCost.Key);

                return (Resource: new Resource(resource), Cost: resourceCost.Value);
            })
            .ToDictionary(resourceCost => resourceCost.Resource, resourceCost => resourceCost.Cost);
    }


    private IReadOnlyList<IWeapon> CreateWeapons
        (
            IReadOnlyDictionary<string, WeaponConfiguration> weapons,
            IReadOnlyDictionary<string, int> weaponsOnUnit
        )
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
                            weaponConfiguration.Cooldown
                        ));
                }
                return createdWeapons;
            }).ToList();
    }


    private IUnit CreatePeacefulUnit
        (
            string id,
            IUser owner,
            StarSystem starSystem,
            Planet? planet,
            IReadOnlyDictionary<Resource, int> resourceCost,
            string type,
            IBuildingFactory? buildingFactory
        )
    {
        var unitStats = _units[type];
        return type switch
        {
            "Scout" =>
                new ScoutUnit(id, owner, starSystem, planet, resourceCost, _clock),
            "Builder" when (buildingFactory is null) =>
                throw new ArgumentException("Builder unit require a BuildingFactory to be built."),
            "Builder" =>
                new BuilderUnit(id, buildingFactory, owner, starSystem, planet, resourceCost, _clock),
            _ =>
                throw new ArgumentException($"Incorrect unit type : {type} is unknown.")
        };
    }
}
