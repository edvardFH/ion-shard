using IonShard.Domain.Map.Locations;
using IonShard.Domain.Units;
using Shard.Shared.Core;

namespace IonShard.Domain.Buildings;

public interface IBuilding
{
    public string Id { get; }
    public string Type { get; }
    public IUnit Builder { get; }
    public ILocation Location { get; }
    public Boolean IsBuilt { get; }
    public Task BuildTask { get; }
    public DateTime? EstimatedBuildTime { get; }

    public void StartBuildBuilding(IClock clock);
    public bool TryRequestBuildStop();
}
