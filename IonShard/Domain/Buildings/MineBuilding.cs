using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using IonShard.Domain.Units;
using IonShard.Utils;

namespace IonShard.Domain.Buildings;

public class MineBuilding : AbstractBuilding
{
    public MineBuilding(IUnit builder, StarSystem starSystem, Planet? planet) : base(builder, starSystem, planet, "mine")
    {
    }
}
