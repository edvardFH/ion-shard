namespace IonShard.Configuration.Units;

public record CombatUnitConfiguration(
    IReadOnlyDictionary<string, int> ResourceCost,
    int BuildingDuration,
    int HealthPoints,
    IReadOnlyDictionary<string, int> Weapons,
    IReadOnlyList<string> CombatPriorities,
    IReadOnlyDictionary<string, float> ShieldDamageReductionMultiplier): IUnitConfiguration;
