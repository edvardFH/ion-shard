using IonShard.Contracts.DTO.Units;
using IonShard.Contracts.RequestBodies;
using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
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
public class UnitsController : ControllerBase
{
    private readonly TimeSpan MaximumWaitingTimeBeforeResponse;


    private readonly UserRepository _usersRepository;
    private readonly MapRepository _mapRepository;
    private readonly IClock _clock;
    private readonly IUnitFactory _unitFactory;
    private readonly IBuildingFactory _buildingFactory;

    public UnitsController(
        UserRepository usersRepository, 
        MapRepository mapRepository,
        IClock clock,
        IConfiguration configuration,
        IUnitFactory unitFactory,
        IBuildingFactory buildingFactory)
    {
        _usersRepository = usersRepository;
        _mapRepository = mapRepository;
        _clock = clock;
        MaximumWaitingTimeBeforeResponse = TimeSpan.FromSeconds(
            configuration.GetValue<int>(
                "Controllers:MaximumWaitingTimeBeforeResponse"));
        _unitFactory = unitFactory;
        _buildingFactory = buildingFactory;
    }


    [HttpGet("{userId}/units")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Returns all units of a user")]
    public ActionResult<IEnumerable<UnitDTO>> GetAllUnitsOfUser(string userId)
    {
        IUser? user = _usersRepository[userId];

        return user is not null
            ? user.Units
                .Values
                .ToList()
                .ConvertAll(unit => unit.ToDTO())
            : NotFound();
    }


    [HttpGet("{userId}/units/{unitId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Returns information about one single unit of a user")]
    public async Task<ActionResult<UnitDTO>> GetOneUnitFromUser(string userId, string unitId)
    {

        IUnit? unit = await GetUnitAsync(userId, unitId);

        return unit is null
            ? NotFound()
            : unit.ToDTO();
    }


    [HttpPut("{userId}/units/{unitId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(Summary = "Change the status of a unit of a user. Right now, only its position (system and planet) can be changed - which is akin to moving it")]
    public ActionResult<UnitDTO?> MoveUnitOfUser(string userId, string unitId, [FromBody] MoveUnitPutRequestBody body)
    {
        if (unitId != body.Id || body.Id is null || body.System is null)
            return BadRequest();

        IUnit? unit = GetUnitFromRepository(userId, unitId);
        
        if(unit is null)
        {
            if (!HttpContext.User.IsInRole("Admin"))
                return Unauthorized();

            if (body.Type is null || !_unitFactory.DoesTypeExist(body.Type))
                return BadRequest();

            IUser? user = _usersRepository[userId];
            StarSystem? starSystem = _mapRepository[body.System];

            if (starSystem is null || user is null)
                return NotFound();

            Planet? planet = body.Planet is null
                ? null
                : _mapRepository[body.System, body.Planet];

            return _unitFactory.CreateUnitWithId
                (
                    body.Id,
                    user,
                    starSystem,
                    planet,
                    body.Type,
                    _buildingFactory
                )
                .ToDTO();
        }


        if (body.DestinationSystem is null)
            return BadRequest();


        StarSystem? destinationSystem = _mapRepository[body.DestinationSystem];

        if (unit is null || destinationSystem is null)
            return NotFound();

        Planet? destinationPlanet = body.DestinationPlanet is not null
            ? destinationSystem[body.DestinationPlanet]
            : null;

        if (destinationPlanet is null && body.DestinationPlanet is not null)
            return NotFound();

        unit.StartTravel(_clock, destinationSystem, destinationPlanet);

        return unit.ToDTO();
    }


    [HttpGet("{userId}/units/{unitId}/location")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Returns more detailed information about the location a unit of user currently is about")]
    public async Task<ActionResult<UnitLocationDTO>> GetUnitLocation(string userId, string unitId)
    {
        IUnit? unit = await GetUnitAsync(userId, unitId);

        return unit is null
            ? NotFound()
            : unit.Location.ToDTO();
    }


    private async Task<IUnit?> GetUnitAsync(string userId, string unitId)
    {
        IUnit? unit = GetUnitFromRepository(userId, unitId);

        if (unit is null)
            return null;

        if (unit.Destination is null)
            return unit;


        TimeSpan unitRemainingTimeOfTravel = unit.Destination.EstimatedTimeOfArrival - _clock.Now;

        if (unitRemainingTimeOfTravel <= MaximumWaitingTimeBeforeResponse)
        {
            await unit.TravelTask;
            return unit;
        }

        return unit;
    }


    private IUnit? GetUnitFromRepository(string userId, string unitId)
    {
        IUser? user = _usersRepository[userId];
        IUnit? unit = null;

        if (user is not null && user.Units.ContainsKey(unitId))
            unit = user.Units[unitId];

        return unit;
    }
}
