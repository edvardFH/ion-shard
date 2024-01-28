using IonShard.Domain.Buildings;
using IonShard.Domain.Buildings.Mine;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Users;
using Shard.Shared.Core;

namespace IonShard.Domain.Units.Builder;

public class BuilderUnit : Unit, IBuilderUnit
{
    private const int BuildBuildingDuration = 300;

    public Task BuildTask { get; private set; }
    public bool IsBuilding => _buildingBeingBuilt is not null;
    public DateTime? EstimatedBuildTime { get; private set; }

    private CancellationTokenSource? _cancellationTokenSource;
    private IBuilding? _buildingBeingBuilt;

    private readonly IBuildingFactory _buildingFactory;

    public BuilderUnit
        (
            string id,
            IBuildingFactory buildingFactory,
            IUser owner,
            StarSystem starSystem,
            Planet? planet,
            IReadOnlyDictionary<Resource, int> resourceCost,
            IClock clock
        )
        : base(id, owner, starSystem, planet, "builder", resourceCost, clock)
    {
        _buildingFactory = buildingFactory;
        BuildTask = Task.CompletedTask;
    }


    public override void StartTravel(IClock clock, StarSystem destinationSystem, Planet? destinationPlanet)
    {
        if (IsBuilding && destinationPlanet != Location.Planet)
            TryRequestBuildStop();

        base.StartTravel(clock, destinationSystem, destinationPlanet);
    }


    public IBuilding StartBuild(IClock clock, string buildingType, ResourceCategory? resourceCategory)
    {
        if (Location.Planet is null)
            throw new InvalidOperationException("Builder must be on a planet to build but its planet location is null.");


        EstimatedBuildTime = clock.Now.AddSeconds(BuildBuildingDuration);
        _buildingBeingBuilt = _buildingFactory.CreateBuilding
            (
                buildingType,
                this,
                Location.System,
                Location.Planet,
                false,
                EstimatedBuildTime,
                resourceCategory
            );

        Owner.AddBuilding(_buildingBeingBuilt);

        _cancellationTokenSource = new CancellationTokenSource();
        BuildTask = BuildAsync(clock, _cancellationTokenSource.Token);

        return _buildingBeingBuilt;
    }


    public bool TryRequestBuildStop()
    {
        if (BuildTask.IsCompleted || _cancellationTokenSource is null || _buildingBeingBuilt is null)
            return false;

        _cancellationTokenSource.Cancel();
        Owner.RemoveBuilding(_buildingBeingBuilt);

        ResetBuildStatus();

        return true;
    }


    public bool DoesBuildingTypeExists(string buildingType) =>
        _buildingFactory.DoesTypeExist(buildingType);


    private async Task BuildAsync(IClock clock, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await clock.Delay(TimeSpan.FromSeconds(BuildBuildingDuration), cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();

        _buildingBeingBuilt?.FinishBuild();

        ResetBuildStatus();
    }


    private void ResetBuildStatus()
    {
        _buildingBeingBuilt = null;
        _cancellationTokenSource = null;
        EstimatedBuildTime = null;
    }
}
