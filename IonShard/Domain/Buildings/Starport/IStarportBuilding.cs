using IonShard.Domain.Units;

namespace IonShard.Domain.Buildings.Starport;

public interface IStarportBuilding : IBuilding
{
    public Queue<IUnit> BuildQueue { get; }
    public IUnit AddToQueue(string unitType);
    public Task BuildTask { get; }
    public bool IsBuilding { get; }
    public DateTime? EstimatedCurrentUnitBuildTime { get; }
}
