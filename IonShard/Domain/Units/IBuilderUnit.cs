using IonShard.Domain.Buildings;

namespace IonShard.Domain.Units;

public interface IBuilderUnit : IUnit
{
    public IBuilding Build(string buildingType);
}
