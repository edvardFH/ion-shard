using IonShard.Configuration.Units;

namespace IonShard.Configuration;

public class GameRulesConfigurationService
{
    private IConfiguration _configuration;

    public GameRulesConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IReadOnlyDictionary<string, WeaponConfiguration> GetWeapons()
    {
        const string key = "Weapons";
        var weapons =
            _configuration.GetValue<IReadOnlyDictionary<string, WeaponConfiguration>>(key);

        if (weapons is null)
            throw new ConfigurationFormatException(key);

        return weapons;
    }

    public IReadOnlyDictionary<string, IUnitConfiguration> GetUnits()
    {
        const string key1 = "Units::PeacefulUnit";
        var units = _configuration
            .GetValue<IDictionary<string, IUnitConfiguration>>(key1);

        if (units is null)
            throw new ConfigurationFormatException(key1);


        const string key2 = "Units::CombatUnit";
        var combatUnit = _configuration
            .GetValue<IReadOnlyDictionary<string, CombatUnitConfiguration>>(key2);

        if (combatUnit is null)
            throw new ConfigurationFormatException(key2);

        combatUnit.ToList().ForEach(combatUnit => units.Add(combatUnit.Key, combatUnit.Value));


        return (IReadOnlyDictionary<string, IUnitConfiguration>)units;
    }
}
