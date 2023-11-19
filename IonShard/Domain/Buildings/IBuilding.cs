using IonShard.Domain.Map.Locations;
using IonShard.Domain.Units;

namespace IonShard.Domain.Buildings;

public interface IBuilding
{
    public string Id { get; }
    public string Type { get; }
    public IBuilderUnit Builder { get; }
    public ILocation Location { get; }
    public bool IsBuilt { get; }

    public void FinishBuild();
}
