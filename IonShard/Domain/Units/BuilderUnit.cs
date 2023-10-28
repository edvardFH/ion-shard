using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using IonShard.Domain.Users;

namespace IonShard.Domain.Units;

public class BuilderUnit : AbstractUnit, IBuilderUnit
{
    public override string Type => "builder";


    public IBuilding Build(string buildingType)
    {
        if (Location.Planet is null)
            throw new Exception("Builder must be on a planet to build.");

        var building = new MineBuilding(this, Location.System, Location.Planet);
        this.Owner.AddBuilding(building);

        return building;
    }


    public BuilderUnit(IUser owner, StarSystem starSystem, Planet? planet) : base(owner, starSystem, planet) { }
}
