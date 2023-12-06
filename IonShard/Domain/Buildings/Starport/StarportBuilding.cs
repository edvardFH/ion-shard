using IonShard.Domain.Map;
using IonShard.Domain.Units;
using IonShard.Domain.Units.Builder;
using Shard.Shared.Core;

namespace IonShard.Domain.Buildings.Starport;

public class StarportBuilding : Building, IStarportBuilding
{
    public Queue<IUnit> BuildQueue { get; }

    public Task BuildTask { get; private set; }
    private readonly IClock _clock;
    private readonly IUnitFactory _unitFactory;

    private IUnit? _unitBeingBuilt = null;
    public bool IsBuilding => _unitBeingBuilt is not null;
    public DateTime? EstimatedCurrentUnitBuildTime { get; }

    public StarportBuilding
        (
            IBuilderUnit builder,
            StarSystem starSystem,
            Planet planet,
            IClock clock,
            IUnitFactory unitFactory,
            DateTime? estimatedBuildTime = null,
            bool isBuilt = false
        )
        : base
        (
            builder,
            starSystem,
            planet,
            "starport",
            estimatedBuildTime,
            isBuilt
        )
    {
        BuildQueue = new Queue<IUnit>();
        BuildTask = Task.CompletedTask;
        _clock = clock;
        _unitFactory = unitFactory;
    }

    public IUnit AddToQueue(string unitType)
    {

        return null;
    }
}
