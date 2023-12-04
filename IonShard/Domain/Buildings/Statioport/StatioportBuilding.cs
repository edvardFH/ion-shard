using IonShard.Domain.Map;
using IonShard.Domain.Units;
using IonShard.Domain.Units.Builder;
using Shard.Shared.Core;

namespace IonShard.Domain.Buildings.Statioport;

public class StatioportBuilding : Building, IStatioportBuilding
{
    public Queue<IUnit> BuildQueue { get; }

    public Task BuildTask { get; private set; }

    private IUnit? _unitBeingBuilt = null;
    public bool IsBuilding => _unitBeingBuilt is not null;
    public DateTime? EstimatedCurrentUnitBuildTime { get; }

    public StatioportBuilding(
        IBuilderUnit builder,
        StarSystem starSystem,
        Planet planet,
        DateTime? estimatedBuildTime = null,
        bool isBuilt = false)
        : base(builder, starSystem, planet, "Statioport", estimatedBuildTime, isBuilt)
    {
        BuildQueue = new Queue<IUnit>();
        BuildTask = Task.CompletedTask;
    }

    public IUnit AddToQueue(IClock clock, string unitType)
    {
        return null;
    }
}
