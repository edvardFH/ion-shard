using IonShard.Domain.Map.Resources;

namespace IonShard.Domain.Buildings;

public interface IMineBuilding : IBuilding
{
    public ResourceCategory ResourceCategory { get; }
}
