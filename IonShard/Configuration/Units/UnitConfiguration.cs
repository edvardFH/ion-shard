namespace IonShard.Configuration.Units;

public record UnitConfiguration(
    IReadOnlyDictionary<string, int> ResourceCost,
    int BuildingDuration)
    : IUnitConfiguration;