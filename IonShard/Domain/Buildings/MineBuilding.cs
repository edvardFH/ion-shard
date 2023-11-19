using IonShard.Domain.Map;
using IonShard.Domain.Units;

namespace IonShard.Domain.Buildings;

public class MineBuilding : AbstractBuilding
{
    public MineBuilding(IBuilderUnit builder, StarSystem starSystem, Planet? planet) : base(builder, starSystem, planet, "mine")
    {
    }
}
