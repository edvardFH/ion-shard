using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units;
using Shard.Shared.Core;

namespace IonShard.Domain.Buildings.Starport;

public interface IStarportBuilding : IBuilding
{
    public Queue<IUnit> BuildQueue { get; }
    public IUnit AddToQueue(string unitType);
    public Task BuildTask { get; }
    public bool IsBuilding { get; }
    public DateTime? EstimatedCurrentUnitBuildTime { get; }
}
