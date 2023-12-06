using IonShard.Domain.Map.Resources;

namespace IonShard.Domain.Buildings.Mine;

public interface IMineBuilding : IBuilding
{
    public ResourceCategory ResourceCategory { get; }
}
