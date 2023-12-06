using IonShard.Domain.Buildings;
using IonShard.Domain.Map.Resources;
using Shard.Shared.Core;

namespace IonShard.Domain.Units.Builder;

public interface IBuilderUnit : IUnit
{
    public IBuilding StartBuild(IClock clock, string buildingType, ResourceCategory? category);
    public Task BuildTask { get; }
    public bool IsBuilding { get; }
    public DateTime? EstimatedBuildTime { get; }

    public bool TryRequestBuildStop();
    public bool DoesBuildingTypeExists(string buildingType);
}