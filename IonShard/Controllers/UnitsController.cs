using IonShard.Contracts.DTO.Units;
using IonShard.Contracts.RequestBodies;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
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

    private readonly UserRepository _usersRepository;
    private readonly MapRepository _mapRepository;
    private readonly IClock _clock;

    public UnitsController(UserRepository usersRepository, MapRepository mapRepository, IClock clock)
    {
        _usersRepository = usersRepository;
        _mapRepository = mapRepository;
        _clock = clock;
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

        IUnit? unit = GetUnitFromRepository(userId, unitId);

        if (unit is null)
            return NotFound();

        if (unit.Destination is null)
            return unit.ToDTO();


        TimeSpan unitRemainingTimeOfTravel = unit.Destination.EstimatedTimeOfArrival - _clock.Now;
        bool unitArrivesSoon = unitRemainingTimeOfTravel <= TimeSpan.FromSeconds(2);

        if (unitArrivesSoon)
        {
            await unit.TravelTask;
            return unit.ToDTO();
        }

        return new UnitDTO(
            unit.Id,
            unit.Type,
            unit.Location.System.Name,
            unit.Location.Planet?.Name,
            unit.Destination.System.Name,
            unit.Destination.Planet?.Name,
            unit.Destination.EstimatedTimeOfArrival.ToString());
    }


    [HttpPut("{userId}/units/{unitId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Change the status of a unit of a user. Right now, only its position (system and planet) can be changed - which is akin to moving it")]
    public ActionResult<UnitDTO?> MoveUnitOfUser(string userId, string unitId, [FromBody] MoveUnitPutRequestBody body)
    {
        if (unitId != body.Id || body.Id is null || body.System is null || body.DestinationSystem is null)
            return BadRequest();

        IUnit? unit = GetUnitFromRepository(userId, unitId);
        StarSystem? system = _mapRepository[body.DestinationSystem];

        if (unit is null || system is null)
            return NotFound();

        Planet? planet = body.DestinationPlanet is not null
            ? system[body.DestinationPlanet]
            : null;

        if (planet is null && body.DestinationPlanet is not null)
            return NotFound();

        unit.Move(_clock, system, planet);

        return unit.ToDTO();
    }


    [HttpGet("{userId}/units/{unitId}/location")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Returns more detailed information about the location a unit of user currently is about")]
    public async Task<ActionResult<UnitLocationDTO>> GetUnitLocation(string userId, string unitId)
    {
        IUnit? unit = GetUnitFromRepository(userId, unitId);

        if (unit is null)
            return NotFound();

        if (unit.Destination is null)
            return unit.Location.ToDTO();


        TimeSpan unitRemainingTimeOfTravel = unit.Destination.EstimatedTimeOfArrival - _clock.Now;
        bool unitArrivesSoon = unitRemainingTimeOfTravel <= TimeSpan.FromSeconds(2);

        if (unitArrivesSoon)
        {
            await unit.TravelTask;
            return unit.Location.ToDTO();
        }

        return new Location(unit.Destination.System, unit.Destination.Planet).ToDTO();
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
