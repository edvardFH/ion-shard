using IonShard.Domain.Map;
using IonShard.Domain.Units;

namespace IonShard.Domain.Buildings;

public interface IBuilding
{
    public string Id { get; }

    public string Type { get; }

    public IUnit Builder { get; }

    public Location Location { get; }
}
