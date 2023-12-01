using IonShard.Domain.Buildings;
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

    public BuilderUnit(
        IUser owner,
        StarSystem starSystem,
        Planet? planet)
        : base(owner, starSystem, planet, "builder")
    {
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

        if (resourceCategory is null)
            throw new ArgumentException("Mine must have a resource category but the provided one is null");


        EstimatedBuildTime = clock.Now.AddSeconds(BuildBuildingDuration);
        _buildingBeingBuilt = new MineBuilding(
            this,
            Location.System,
            Location.Planet,
            (ResourceCategory)resourceCategory,
            clock,
            EstimatedBuildTime);

        Owner.AddBuilding(_buildingBeingBuilt);

        _cancellationTokenSource = new CancellationTokenSource();
        BuildTask = BuildAsync(clock, _cancellationTokenSource.Token);

        return _buildingBeingBuilt;
    }


    private async Task BuildAsync(IClock clock, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await clock.Delay(TimeSpan.FromSeconds(BuildBuildingDuration), cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();

        _buildingBeingBuilt?.FinishBuild();

        ResetBuildStatus();
    }

    public bool TryRequestBuildStop()
    {
        if (BuildTask.IsCompleted || _cancellationTokenSource is null || _buildingBeingBuilt is null)
            return false;

        _cancellationTokenSource.Cancel();
        Owner.RemoveBuilding(_buildingBeingBuilt.Id);

        ResetBuildStatus();

        return true;
    }

    private void ResetBuildStatus()
    {
        _buildingBeingBuilt = null;
        _cancellationTokenSource = null;
        EstimatedBuildTime = null;
    }
}
