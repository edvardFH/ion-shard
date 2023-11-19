using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using IonShard.Domain.Units;
using IonShard.Utils;
using Shard.Shared.Core;

namespace IonShard.Domain.Buildings;

public abstract class AbstractBuilding : IBuilding
{
    private const int BuildBuildingDuration = 300;

    public string Id { get; }
    public string Type { get; }
    public IBuilderUnit Builder { get; }
    public ILocation Location { get; }
    public bool IsBuilt { get; private set; }

    public AbstractBuilding(IBuilderUnit builder, StarSystem starSystem, Planet? planet, String type, bool isBuilt = false)
    {
        Id = new Random().NextGuid().ToString();
        Builder = builder;
        Location = new Location(starSystem, planet);
        Type = type;
        IsBuilt = isBuilt;
    }

    public void FinishBuild()
    {
        if (IsBuilt)
            throw new InvalidOperationException("Building is already built.");

        IsBuilt = true;
    }
}