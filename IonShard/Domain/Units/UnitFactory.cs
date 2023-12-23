using IonShard.Configuration;
using IonShard.Configuration.Gamerules;
using IonShard.Configuration.Gamerules.Units;
using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units.Builder;
using IonShard.Domain.Units.Cargo;
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
    private readonly IResourceFactory _resourceFactory;


    public UnitFactory(IGameRulesService gameRulesService, IClock clock, IResourceFactory resourceFactory)
    {
        _weapons = gameRulesService.Weapons;
        _units = gameRulesService.Units;
        _clock = clock;
        _resourceFactory = resourceFactory;
    }


    public IUnit CreateUnitWithId
        (
            string id,
            IUser owner,
            StarSystem system,
            Planet? planet,
            string type,
            IBuildingFactory? buildingFactory,
            int healthPoints = -1,
            IReadOnlyDictionary<IResource, int>? loadedResources = null
        )
    {
        var unitConfig = GetUnitConfiguration(type);

        var unitHealthPoints = healthPoints >= 0 ? healthPoints : unitConfig.HealthPoints;

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
                    type.UppercaseFirstWord(),
                    unitHealthPoints,
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
                    type.UppercaseFirstWord(),
                    unitHealthPoints,
                    buildingFactory,
                    loadedResources
                )
        };

        return newUnit;
    }

    public IUnit CreateNewUnit
        (
            IUser owner,
            StarSystem system,
            Planet? planet,
            string type,
            IBuildingFactory? buildingFactory
        )
    {
        var unitConfig = GetUnitConfiguration(type);

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


    private IReadOnlyDictionary<IResource, int> CreateResourceCost(IUnitConfiguration unitConfiguration)
    {
        return unitConfiguration.ResourceCost
            .Select(resourceCost =>
            (
                Resource: _resourceFactory.GetResource(resourceCost.Key),
                Cost: resourceCost.Value)
            )
            .ToDictionary(resourceCost => resourceCost.Resource, resourceCost => resourceCost.Cost);
    }


    private IUnitConfiguration GetUnitConfiguration(string type)
    {
        var formattedType = type.UppercaseFirstWord();

        if (!DoesTypeExist(formattedType))
            throw new ArgumentException(
                $"Incorrect unit type : {formattedType} is not contained in game rules.");

        return _units[formattedType];
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
            IReadOnlyDictionary<IResource, int> resourceCost,
            string type,
            int healthPoints,
            IBuildingFactory? buildingFactory,
            IReadOnlyDictionary<IResource, int>? loadedResources
        )
    {
        var completeResources = _resourceFactory.CompleteWithMissingResources(loadedResources ?? new Dictionary<IResource, int>());
        return type switch
        {
            "Scout" =>
                new ScoutUnit(id, owner, starSystem, planet, resourceCost, healthPoints, _clock),
            "Builder" when (buildingFactory is null) =>
                throw new ArgumentException("Builder unit require a BuildingFactory to be built."),
            "Builder" =>
                new BuilderUnit(id, buildingFactory, owner, starSystem, planet, resourceCost, healthPoints, _clock),
            "Cargo" =>
                new CargoUnit(id, owner, starSystem, planet, type, resourceCost, healthPoints, completeResources.AsReadOnly(), _clock),
            _ =>
                throw new ArgumentException($"Incorrect unit type : {type} is unknown.")
        };
    }
}
