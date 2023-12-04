using IonShard.Configuration;
using IonShard.Domain.Buildings.Mine;
using IonShard.Domain.Buildings.Statioport;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units.Builder;
using Shard.Shared.Core;

namespace IonShard.Domain.Buildings;

public class BuildingFactory : IBuildingFactory
{
    private IReadOnlyDictionary<string, BuildingConfiguration> _buildings;
    private IClock _clock;


    public BuildingFactory(IGameRulesService gameRuleService, IClock clock)
    {
        _buildings = gameRuleService.GetBuildings();
        _clock = clock;
    }


    public IBuilding GetBuildingWith
        (
            string type,
            IBuilderUnit builder,
            StarSystem starSystem,
            Planet planet,
            bool isBuilt = false,
            DateTime? estimatedBuildTime = null,
            ResourceCategory? resourceCategory = null
        )
    {
        if (!DoesTypeExist(type))
            throw new ArgumentException(
                $"Incorrect building type : {type} is not contained in game rules.");

        var buildingStats = _buildings[type];

        return type switch
        {
            "Mine" when (resourceCategory is null) =>
                throw new ArgumentException
                (
                    "Mine type was passed but not resource category was specified."
                ),
            "Mine" when (resourceCategory is ResourceCategory category) =>
                new MineBuilding
                (
                    builder,
                    starSystem,
                    planet,
                    category,
                    _clock,
                    estimatedBuildTime,
                    isBuilt
                ),
            "Statioport" =>
                new StatioportBuilding
                (
                    builder,
                    starSystem,
                    planet,
                    estimatedBuildTime,
                    isBuilt
                ),
            _ =>
                throw new NotImplementedException
                (
                    $"{type} exists in game rules but is not implemented."
                )
        };
    }


    public bool DoesTypeExist(string type) => _buildings.ContainsKey(type);
}
