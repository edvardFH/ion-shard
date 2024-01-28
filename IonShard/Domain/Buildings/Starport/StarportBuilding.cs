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
    private readonly IBuildingFactory _buildingFactory;

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
            IBuildingFactory buildingFactory,
            DateTime? estimatedBuildTime = null,
            bool isBuilt = false
        )
        : base
        (
            builder,
            starSystem,
            planet,
            "Starport",
            estimatedBuildTime,
            isBuilt
        )
    {
        BuildQueue = new Queue<IUnit>();
        BuildTask = Task.CompletedTask;
        _clock = clock;
        _unitFactory = unitFactory;
        _buildingFactory = buildingFactory;
    }

    public IUnit AddToQueue(string unitType)
    {
        if (!Builder.Owner.HasResourcesFor(unitType))
            throw new InvalidOperationException(
                $"User {Builder.Owner.Id} does not have enough resources to create {unitType}");



        var newUnit = _unitFactory.CreateNewUnit(
            Builder.Owner,
            Location.System,
            Location.Planet,
            unitType,
            _buildingFactory);

        newUnit.ResourceCost
            .ToList()
            .ForEach(resourceCost => Builder.Owner.UseResource(resourceCost.Key, resourceCost.Value));

        return newUnit;
    }
}
