using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using IonShard.Domain.Units;
using IonShard.Utils;
using Shard.Shared.Core;

namespace IonShard.Domain.Buildings;

public abstract class AbstractBuilding : IBuilding
{
    protected const int BuildingBuildDuration = 300;

    public string Id { get; }
    public string Type { get; }
    public IBuilderUnit Builder { get; }
    public ILocation Location { get; }
    public DateTime? EstimatedBuildTime { get; private set; }
    public bool IsBuilt { get; private set; }

    public AbstractBuilding(
        IBuilderUnit builder,
        StarSystem starSystem,
        Planet planet,
        String type,
        DateTime? estimatedBuildTime = null,
        bool isBuilt = false)
    {
        Id = new Random().NextGuid().ToString();
        Builder = builder;
        Location = new Location(starSystem, planet);
        Type = type;
        IsBuilt = isBuilt;
        EstimatedBuildTime = estimatedBuildTime;
    }

    public void FinishBuild()
    {
        if (IsBuilt)
            throw new InvalidOperationException("Building is already built.");

        EstimatedBuildTime = null;
        IsBuilt = true;
    }
}