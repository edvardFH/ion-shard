using IonShard.Configuration;
using IonShard.Domain.Buildings.Mine;
using IonShard.Domain.Buildings.Statioport;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units;
using IonShard.Domain.Units.Builder;
using IonShard.Utils;
using Shard.Shared.Core;

namespace IonShard.Domain.Buildings;

public class BuildingFactory : IBuildingFactory
{
    private readonly IReadOnlyDictionary<string, BuildingConfiguration> _buildings;
    private readonly IClock _clock;
    private IUnitFactory _unitFactory;


    public BuildingFactory
        (
            IGameRulesService gameRuleService,
            IClock clock,
            IUnitFactory unitFactory
        )
    {
        _buildings = gameRuleService.GetBuildings();
        _clock = clock;
        _unitFactory = unitFactory;
    }


    public IBuilding CreateBuilding
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
        var formattedType = type.UppercaseFirstWord();

        if (!DoesTypeExist(formattedType))
            throw new ArgumentException(
                $"Incorrect building type : {formattedType} is not contained in game rules.");

        var buildingStats = _buildings[formattedType];

        return formattedType switch
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
                    _clock,
                    _unitFactory,
                    estimatedBuildTime,
                    isBuilt
                ),
            _ =>
                throw new NotImplementedException
                (
                    $"{formattedType} exists in game rules but is not implemented."
                )
        };
    }


    public bool DoesTypeExist(string type) =>
        _buildings.ContainsKey(type.UppercaseFirstWord());
}
