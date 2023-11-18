using IonShard.Contracts.DTO.Buildings;
using IonShard.Contracts.RequestBodies;
using IonShard.Domain.Buildings;
using IonShard.Domain.Units;
using IonShard.Domain.Users;
using IonShard.Mappers;
using IonShard.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;
using Swashbuckle.AspNetCore.Annotations;

namespace IonShard.Controllers;

[Route("Users")]
[ApiController]
[Produces("application/json")]
public class BuildingsController : ControllerBase
{
    private readonly TimeSpan MaximumWaitingTimeBeforeResponse = new TimeSpan(0, 0, 2); // TODO: move to conf becasue object defined twice
    
    private readonly UserRepository _usersRepository;
    private readonly IClock _clock;

    public BuildingsController(UserRepository usersRepository, IClock clock)
    {
        _usersRepository = usersRepository;
        _clock = clock;
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

        if (body is null || body.BuilderId is null || body.Type != "mine")
            return BadRequest();


        IUnit? unit = user.Units.ContainsKey(body.BuilderId)
            ? user.Units[body.BuilderId]
            : null;

        if (unit is null || unit is not IBuilderUnit || unit.Location.Planet is null)
            return BadRequest();


        IBuilderUnit builder = (IBuilderUnit)unit;
        IBuilding building = builder.Build(_clock, body.Type);

        return building.ToDTO();
    }


    [HttpGet("{userId}/buildings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Return all buildings of a user")]
    public ActionResult<List<BuildingDTO>> Get(string userId)
    {
        IUser? user = _usersRepository[userId];

        return user is not null
            ? user.Buildings
                .Values
                .ToList()
                .ConvertAll(building => building.ToDTO())
            : NotFound();
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


        var buildingRemainingBuildTime = building.EstimatedBuildTime - _clock.Now;

        if (buildingRemainingBuildTime > MaximumWaitingTimeBeforeResponse) 
            return building.ToDTO();

        
        await building.BuildTask;
        return building.ToDTO();
    }
}
