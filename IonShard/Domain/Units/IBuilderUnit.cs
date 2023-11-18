using IonShard.Domain.Buildings;
using Shard.Shared.Core;

namespace IonShard.Domain.Units;

public interface IBuilderUnit : IUnit
{
    public IBuilding Build(IClock clock, string buildingType);
}
