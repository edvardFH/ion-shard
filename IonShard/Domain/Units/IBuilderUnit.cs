using IonShard.Domain.Buildings;
using Shard.Shared.Core;

namespace IonShard.Domain.Units;

public interface IBuilderUnit : IUnit
{
    public IBuilding StartBuild(IClock clock, string buildingType);
    public Task BuildTask { get; }
    public bool IsBuilding { get; }
    public DateTime? EstimatedBuildTime { get; }
}