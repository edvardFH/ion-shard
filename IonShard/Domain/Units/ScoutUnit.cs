using IonShard.Domain.Map;

namespace IonShard.Domain.Units;

public class ScoutUnit : AbstractUnit
{
    public override string Type => "scout";

    public ScoutUnit(string id, StarSystem starSystem, Planet? planet) : base(id, starSystem, planet) { }
}
