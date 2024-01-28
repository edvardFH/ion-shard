using IonShard.Domain.Map.Locations;

namespace IonShard.Domain.Units.Scout;

public interface IScoutUnit: IUnit
{
    public new ILocationWithDetails Location { get; }
}
