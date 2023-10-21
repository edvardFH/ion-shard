using IonShard.Domain.Buildings;

namespace IonShard.Domain.Units;

public interface IBuilder : IUnit
{
    public Building Build(string buildingType);
}
