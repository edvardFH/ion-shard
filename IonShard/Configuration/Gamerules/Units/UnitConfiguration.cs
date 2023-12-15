namespace IonShard.Configuration.Gamerules.Units;

public record UnitConfiguration(
    IReadOnlyDictionary<string, int> ResourceCost,
    int BuildingDuration)
    : IUnitConfiguration;