using IonShard.Configuration.Gamerules.Units;
using System.Collections.ObjectModel;

namespace IonShard.Configuration.Gamerules;

public class GameRulesService : IGameRulesService
{
    private readonly IConfiguration _configuration;
    public IReadOnlyDictionary<string, IUnitConfiguration> Units { get; private set; }


    public GameRulesService(IConfiguration configuration)
    {
        _configuration = configuration;
        Units = InitUnits();
    }


    public IReadOnlyDictionary<string, WeaponConfiguration> GetWeapons()
    {
        const string key = "Weapons";
        var weapons = _configuration
            .GetSection(key)
            .Get<IReadOnlyDictionary<string, WeaponConfiguration>>();

        if (weapons is null)
            throw new ConfigurationFormatException(key);

        return weapons;
    }


    public IReadOnlyDictionary<string, IUnitConfiguration> InitUnits()
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
                        GetAsIntDictionnary(unit, "ResourceCost"),
                        GetAsInt(unit, "BuildingDuration")
                    )
                )
            ).ToDictionary(
                keyValuePair => keyValuePair.Key,
                keyValuePair => (IUnitConfiguration)keyValuePair.Value);


        if (units is null) // TODO : check if enumerable only contains not null value
            throw new ConfigurationFormatException(peacefulUnitSectionKey);


        const string combatUnitSectionKey = "Units:CombatUnit";
        var combatUnitSectionChildren = _configuration
            .GetSection(combatUnitSectionKey)
            .GetChildren();

        if (combatUnitSectionChildren is null)
            throw new ConfigurationFormatException(combatUnitSectionKey);

        List<(string Key, IUnitConfiguration Value)> combatUnits =
             combatUnitSectionChildren
             .Select(unit =>
                 (
                     unit.Key,
                     Value: (IUnitConfiguration)new CombatUnitConfiguration
                     (
                         GetAsIntDictionnary(unit, "ResourceCost"),
                         GetAsInt(unit, "BuildingDuration"),
                         GetAsInt(unit, "HealthPoints"),
                         GetAsIntDictionnary(unit, "Weapons"),
                         GetAsStringList(unit, "CombatPriorities"),
                         GetAsFloatDictionnary(unit, "ShieldDamageReductionMultiplier")
                     )
                 )
             ).ToList<(string, IUnitConfiguration)>();


        if (combatUnits is null)
            throw new ConfigurationFormatException(combatUnitSectionKey);

        combatUnits.ForEach(keyValuePair => units.Add(keyValuePair.Key, keyValuePair.Value));

        return units;
    }


    public IReadOnlyDictionary<string, BuildingConfiguration> GetBuildings()
    {
        const string key = "Buildings";
        var buildings = _configuration
            .GetSection(key)
            .Get<IReadOnlyDictionary<string, BuildingConfiguration>>();

        if (buildings is null)
            throw new ConfigurationFormatException(key);

        return buildings;
    }


    private int GetAsInt(IConfigurationSection unit, string key) =>
        unit.GetSection(key).Get<int>();

    private IReadOnlyDictionary<string, int> GetAsIntDictionnary(IConfigurationSection section, string key) =>
        section.GetSection(key)
            .Get<IReadOnlyDictionary<string, int>>()
            ?? new Dictionary<string, int>();

    private IReadOnlyDictionary<string, float> GetAsFloatDictionnary(IConfigurationSection section, string key) =>
        section.GetSection(key)
            .Get<IReadOnlyDictionary<string, float>>()
            ?? new Dictionary<string, float>();

    private IReadOnlyList<string> GetAsStringList(IConfigurationSection section, string key) =>
        section.GetSection(key)
            .Get<IReadOnlyList<string>>()
            ?? new List<string>();
}
