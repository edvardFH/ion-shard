using IonShard.Configuration.Gamerules.Buildings;
using IonShard.Configuration.Gamerules.Resources;
using IonShard.Configuration.Gamerules.Units;
using IonShard.Configuration.Gamerules.Users;

namespace IonShard.Configuration.Gamerules;

public class GameRulesService : IGameRulesService
{
    private const float DefaultDamageReductionMutliplier = 1;
    private const int DefaultResourceQuantity = 0;

    private readonly IConfiguration _configuration;
    public IReadOnlyDictionary<string, ResourceConfiguration> Resources { get; private set; }
    public IReadOnlyDictionary<string, IUnitConfiguration> Units { get; private set; }
    public IReadOnlyDictionary<string, WeaponConfiguration> Weapons { get; private set; }
    public IReadOnlyDictionary<string, BuildingConfiguration> Buildings { get; private set; }
    public UserConfiguration User {  get; private set; }


    public GameRulesService(IConfiguration configuration)
    {
        _configuration = configuration;
        Resources = InitResources();
        Units = InitUnits();
        Weapons = InitWeapons();
        Buildings = InitBuildings();
        User = InitUser();
    }


    private IReadOnlyDictionary<string, ResourceConfiguration> InitResources()
    {
        const string key = "Resources";
        var resources = _configuration
            .GetSection(key)
            .Get<IReadOnlyDictionary<string, ResourceConfiguration>>();

        if (resources is null || resources.Count == 0)
            throw new ConfigurationFormatException(key);

        return resources;
    }



    private IReadOnlyDictionary<string, WeaponConfiguration> InitWeapons()
    {
        const string key = "Weapons";
        var weapons = _configuration
            .GetSection(key)
            .Get<IReadOnlyDictionary<string, WeaponConfiguration>>();

        if (weapons is null || weapons.Count == 0)
            throw new ConfigurationFormatException(key);

        return weapons;
    }



    private IReadOnlyDictionary<string, BuildingConfiguration> InitBuildings()
    {
        const string key = "Buildings";
        var buildings = _configuration
            .GetSection(key)
            .Get<IReadOnlyDictionary<string, BuildingConfiguration>>();

        if (buildings is null || buildings.Count == 0)
            throw new ConfigurationFormatException(key);

        return buildings;
    }



    private IReadOnlyDictionary<string, IUnitConfiguration> InitUnits()
    {
        const string peacefulUnitSectionKey = "Units:PeacefulUnit";
        var peacefulUnitSectionChildren = _configuration
            .GetSection(peacefulUnitSectionKey)
            .GetChildren();

        if (peacefulUnitSectionChildren is null)
            throw new ConfigurationFormatException(peacefulUnitSectionKey);

        Dictionary<string, IUnitConfiguration> units =
            peacefulUnitSectionChildren
            .Select(unit =>
                (
                    unit.Key,
                    Value: new UnitConfiguration
                    (
                        unit.GetSectionAsIntDictionnary("ResourceCost"),
                        unit.GetSectionAsInt("BuildingDuration"),
                        unit.GetSectionAsInt("HealthPoints")
                    )
                )
            ).ToDictionary(
                keyValuePair => keyValuePair.Key,
                keyValuePair => (IUnitConfiguration)keyValuePair.Value);


        if (units is null) // TODO : check if enumerable only contains not null value
            throw new ConfigurationFormatException(peacefulUnitSectionKey);


        var combatUnits = CreateCombatUnitsConfiguration();

        combatUnits.ForEach(keyValuePair => units.Add(keyValuePair.Key, keyValuePair.Value));

        return units;
    }


    private List<(string Key, IUnitConfiguration Value)> CreateCombatUnitsConfiguration()
    {
        const string combatUnitSectionKey = "Units:CombatUnit";
        var combatUnitSectionChildren = _configuration
            .GetSection(combatUnitSectionKey)
            .GetChildren();

        var combatUnitTypes = combatUnitSectionChildren.Select(unit => unit.Key);


        if (combatUnitSectionChildren is null)
            throw new ConfigurationFormatException(combatUnitSectionKey);

        List<(string Key, IUnitConfiguration Value)> combatUnits =
             combatUnitSectionChildren
             .Select(unit =>
             {
                 return
                 (
                     unit.Key,
                     Value: (IUnitConfiguration)new CombatUnitConfiguration
                     (
                         unit.GetSectionAsIntDictionnary("ResourceCost"),
                         unit.GetSectionAsInt("BuildingDuration"),
                         unit.GetSectionAsInt("HealthPoints"),
                         unit.GetSectionAsIntDictionnary("Weapons"),
                         unit.GetSectionAsStringList("CombatPriorities"),
                         GetDamageReductionMultipliers(combatUnitTypes, unit)
                     )
                 );
             }

             ).ToList<(string, IUnitConfiguration)>();


        if (combatUnits is null)
            throw new ConfigurationFormatException(combatUnitSectionKey);

        return combatUnits;
    }


    private IReadOnlyDictionary<string, float> GetDamageReductionMultipliers
        (
            IEnumerable<string> combatUnitTypes,
            IConfigurationSection unit
        )
    {
        var damageReductionMutipliers = new Dictionary<string, float>
            (
                unit.GetSectionAsFloatDictionnary("DamageReductionMultipliers")
            );

        combatUnitTypes.ToList().ForEach(type =>
        {
            if (!damageReductionMutipliers.ContainsKey(type))
                damageReductionMutipliers.Add(type, DefaultDamageReductionMutliplier);
        });

        return damageReductionMutipliers;
    }


    private UserConfiguration InitUser()
    {
        const string key = "Users";
        var userSection = _configuration
            .GetSection(key);

        if (userSection is null)
            throw new ConfigurationFormatException(key);

        var startingResources = new Dictionary<string, int>(userSection.GetSectionAsIntDictionnary("StartingResources"));

        if (startingResources.Count == 0)
            throw new ConfigurationFormatException("StartingResources");

        Resources.ToList().ForEach(resource =>
        {
            if (!startingResources.ContainsKey(resource.Key))
                startingResources.Add(resource.Key, DefaultResourceQuantity);
        });

        return new UserConfiguration(startingResources);
    }
}
