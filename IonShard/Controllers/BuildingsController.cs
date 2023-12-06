using IonShard.Contracts.DTO.Buildings;
using IonShard.Contracts.DTO.Units;
using IonShard.Contracts.RequestBodies;
using IonShard.Domain.Buildings;
using IonShard.Domain.Buildings.Starport;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units;
using IonShard.Domain.Units.Builder;
using IonShard.Domain.Users;
using IonShard.Mappers;
using IonShard.Persistence.Repositories;
using IonShard.Utils;
using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;
using Swashbuckle.AspNetCore.Annotations;

namespace IonShard.Controllers;

[Route("Users")]
[ApiController]
[Produces("application/json")]
public class BuildingsController : ControllerBase
{
    private readonly TimeSpan MaximumWaitingTimeBeforeResponse;

    private readonly UserRepository _usersRepository;
    private readonly IClock _clock;


    public BuildingsController(
        UserRepository usersRepository,
        IClock clock,
        IConfiguration configuration)
    {
        _usersRepository = usersRepository;
        _clock = clock;
        MaximumWaitingTimeBeforeResponse = TimeSpan.FromSeconds(
            configuration.GetValue<int>(
                "Controllers:MaximumWaitingTimeBeforeResponse"));
    }


    [HttpPost("{userId}/buildings")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Creates a building at a location")]
    public ActionResult<BuildingDTO> Post(string userId, [FromBody] CreateBuildingPostRequestBody body)
    {
        IUser? user = _usersRepository[userId];

        if (user is null)
            return NotFound();

        if (body is null || body.BuilderId is null || body.Type is null)
            return BadRequest();


        if (body.Type is "mine" && body.ResourceCategory is null)
            return BadRequest();

        if (!Enum.TryParse(
                body.ResourceCategory?.UppercaseFirstWord(),
                out ResourceCategory category) && body.Type is "mine")
            return BadRequest("Invalid resource category");

        IUnit? unit = user.Units.ContainsKey(body.BuilderId)
            ? user.Units[body.BuilderId]
            : null;

        if (unit is not IBuilderUnit builder || unit.Location.Planet is null)
            return BadRequest();

        try
        {
            return builder.StartBuild(_clock, body.Type, category).ToDTO();
        }
        catch
        {
            return BadRequest();
        }
    }


    [HttpGet("{userId}/buildings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Return all buildings of a user")]
    public ActionResult<List<BuildingDTO>> Get(string userId)
    {
        IUser? user = _usersRepository[userId];

        return user is null
            ? NotFound()
            : user.Buildings
                .Values
                .ToList()
                .ConvertAll(building => building.ToDTO());
    }


    [HttpGet("{userId}/buildings/{buildingId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Return information about one single building of a user")]
    public async Task<ActionResult<BuildingDTO>> GetBuildingById(string userId, string buildingId)
    {
        IUser? user = _usersRepository[userId];

        if (user is null)
            return NotFound("No user with such id");


        IBuilding? building = user.Buildings.ContainsKey(buildingId)
            ? user.Buildings[buildingId]
            : null;

        if (building is null)
            return NotFound("User does not have a building with such id");

        if (building.IsBuilt)
            return building.ToDTO();


        var buildingRemainingBuildTime = building.Builder.EstimatedBuildTime - _clock.Now;

        if (buildingRemainingBuildTime > MaximumWaitingTimeBeforeResponse)
            return building.ToDTO();

        try
        {
            await building.Builder.BuildTask;
            return building.ToDTO();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpPost("{userId}/buildings/{starportId}/queue")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Add a unit to the build queue of the starport. Currently immediatly returns the unit.")]
    public ActionResult<UnitDTO> PostToQueue(string userId, string starportId, [FromBody] UnitBlueprintDTO unitBlueprint)
    {
        IUser? user = _usersRepository[userId];

        if (user is null)
            return NotFound("No user with such id");


        IBuilding? building = user.Buildings.ContainsKey(starportId)
            ? user.Buildings[starportId]
            : null;

        if (building is null)
            return NotFound("User does not have a building with such id");

        if (building is not IStarportBuilding starport)
            return BadRequest("Unit must be a starport");

        if (unitBlueprint?.Type is not string unitType)
            return BadRequest("Body should contains a unit type.");

        if (!user.HasResourcesFor(unitType))
            return BadRequest($"User does not have enough resources to create {unitType}.");

        return starport.AddToQueue(unitType).ToDTO();
    }
}
