using IonShard.Domain.Map;
using IonShard.Domain.Units;
using Shard.Shared.Core;

namespace IonShard.Domain.Buildings;

public class StartPortBuilding: AbstractBuilding, IStarPortBuilding
{
    public Queue<IUnit> BuildQueue { get; }
   
    public Task BuildTask { get; private set; }

    private IUnit? _unitBeingBuilt = null;
    public bool IsBuilding => _unitBeingBuilt is not null;
    public DateTime? EstimatedCurrentUnitBuildTime { get; }

    public StartPortBuilding(
        IBuilderUnit builder,
        StarSystem starSystem,
        Planet planet,
        DateTime? estimatedBuildTime = null,
        bool isBuilt = false)
        : base(builder, starSystem, planet, "starPort", estimatedBuildTime, isBuilt)
    {
        BuildQueue = new Queue<IUnit>();
        BuildTask = Task.CompletedTask;
    }

    public IUnit AddToQueue(IClock clock, string unitType)
    {
        return null;
    }
}
