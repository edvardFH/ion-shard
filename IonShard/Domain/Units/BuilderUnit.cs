using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Users;
using Shard.Shared.Core;

namespace IonShard.Domain.Units;

public class BuilderUnit : AbstractUnit, IBuilderUnit
{
    public override string Type => "builder";

    public override void StartTravel(IClock clock, StarSystem destinationSystem, Planet? destinationPlanet)
    {
        foreach (var building in GetBuildingsWhereBuildInProgress().Where(building => building.TryRequestBuildStop()))
        {
            Owner.RemoveBuilding(Id);
        }

        base.StartTravel(clock, destinationSystem, destinationPlanet);
    }

    public IBuilding Build(string buildingType)
    {
        if (Location.Planet is null)
            throw new InvalidOperationException("Builder must be on a planet to build but it's planet location is null.");

        var building = new MineBuilding(this, Location.System, Location.Planet);
        this.Owner.AddBuilding(building);

        return building;
    }


    public BuilderUnit(IUser owner, StarSystem starSystem, Planet? planet) : base(owner, starSystem, planet) { }

    private List<IBuilding> GetBuildingsWhereBuildInProgress()
        => Owner.Buildings.Values.Where(building => 
            building is { IsBuilt: false, BuildTask.Status: TaskStatus.WaitingForActivation or TaskStatus.Running }
            && building.Builder == this )
            .ToList();
    
}
