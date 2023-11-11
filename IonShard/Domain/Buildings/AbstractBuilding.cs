using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using IonShard.Domain.Units;
using IonShard.Utils;
using Shard.Shared.Core;

namespace IonShard.Domain.Buildings;

public class AbstractBuilding : IBuilding
{
    public string Id { get; protected set; }
    public string Type { get; protected set; }
    public IUnit Builder { get; protected set; }
    public ILocation Location { get; protected set; }
    public bool IsBuilt { get; protected set; }
    public Task BuildTask { get; protected set; }
    public DateTime EstimatedBuildTime { get; protected set; }
    private CancellationTokenSource _cancellationTokenSource;

    protected AbstractBuilding(IUnit builder, StarSystem starSystem, Planet? planet, String type)
    {
        Id = new Random().NextGuid().ToString();
        Builder = builder;
        Location = new Location(starSystem, planet);
        Type = type;
        IsBuilt = false;
        BuildTask = Task.CompletedTask;
    }
    
    public void StartBuildBuilding(IClock clock)
    {
        if (!IsBuilt)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            BuildTask = BuildBuilding(clock, _cancellationTokenSource.Token);
        }
    }
    
    private async Task BuildBuilding(IClock clock, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        EstimatedBuildTime = clock.Now.AddMinutes(5);
        await clock.Delay(TimeSpan.FromMinutes(5), cancellationToken);
        IsBuilt = true;
    }

    public bool TryRequestBuildStop()
    {
        var cancellationSuccessfullyRequested = false;

        if (BuildTask.Status is TaskStatus.Running or TaskStatus.WaitingForActivation && _cancellationTokenSource is not null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
            cancellationSuccessfullyRequested = true;
        }

        return cancellationSuccessfullyRequested;
    }
}