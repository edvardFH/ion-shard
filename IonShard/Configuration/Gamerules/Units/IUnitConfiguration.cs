namespace IonShard.Configuration.Gamerules.Units;

public interface IUnitConfiguration
{
    public IReadOnlyDictionary<string, int> ResourceCost { get; }
    public int BuildingDuration { get; }

    public int HealthPoints {  get; }
}
