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
    private readonly TimeSpan MaximumWaitingTimeBeforeResponse = new TimeSpan(0, 0, 2);
    
    private readonly UserRepository _usersRepository;
    private readonly IClock _clock;

    public BuildingsController(UserRepository usersRepository, IClock clock)
    {
        _usersRepository = usersRepository;
        _clock = clock;
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

        IBuilding building = builder.Build(body.Type);
        building.StartBuildBuilding(_clock);
        return building.ToDTO();
    }

    [HttpGet("{userId}/buildings/{buildingId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Return information about one single building of a user")]
    public async Task<ActionResult<BuildingDTO>> GetBuildingById(string userId, string buildingId)
    {
        IUser? user = _usersRepository[userId];
        IBuilding? building = user?.Buildings[buildingId];
        
        if (user is null || building is null)
            return NotFound();

        TimeSpan buildingRemainingBuildTime = building.EstimatedBuildTime - _clock.Now;
        if (building.IsBuilt || buildingRemainingBuildTime > MaximumWaitingTimeBeforeResponse) 
            return building.ToDTO();
        
        await building.BuildTask;
        return building.ToDTO();
    }
}
