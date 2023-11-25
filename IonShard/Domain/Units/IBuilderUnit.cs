using IonShard.Domain.Buildings;
using IonShard.Domain.Map.Resources;
using Shard.Shared.Core;

namespace IonShard.Domain.Units;

public interface IBuilderUnit : IUnit
{
    public IBuilding StartBuild(IClock clock, string buildingType, ResourceCategory? category);
    public Task BuildTask { get; }
    public bool IsBuilding { get; }
    public DateTime? EstimatedBuildTime { get; }
}