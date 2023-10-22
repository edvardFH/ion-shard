using IonShard.Contracts.DTO.Units;
using IonShard.Contracts.RequestBodies;
using IonShard.Domain.Map;
using IonShard.Domain.Units;
using IonShard.Domain.Users;
using IonShard.Mappers;
using IonShard.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace IonShard.Controllers;

[Route("users")]
[ApiController]
[Produces("application/json")]
public class UnitsController : ControllerBase
{
    
    private readonly UserRepository _usersRepository;
    private readonly MapRepository _mapRepository;

    public UnitsController(UserRepository usersRepository, MapRepository mapRepository)
    {
        _usersRepository = usersRepository;
        _mapRepository = mapRepository;
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
    public ActionResult<UnitDTO> GetOneUnitFromUser(string userId, string unitId)
    {
        IUnit? unit = GetUnitFromRepository(userId, unitId);

        return unit is not null
            ? unit.ToDTO()
            : NotFound();
    }


    [HttpPut("{userId}/units/{unitId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Change the status of a unit of a user. Right now, only its position (system and planet) can be changed - which is akin to moving it")]
    public ActionResult<UnitDTO?> MoveUnitOfUser(string userId, string unitId, [FromBody] MoveUnitPutRequestBody body)
    {
        if (unitId != body.Id || body.Id is null || body.System is null)
            return BadRequest();

        IUnit? unit = GetUnitFromRepository(userId, unitId);
        Location? location = unit?.Location;
        StarSystem? system = _mapRepository[body.System];
            
        if (unit is null || location is null || system is null)
            return NotFound();
            
        Planet? planet = body.Planet is not null
            ? system?[body.Planet]
            : null;

        if (planet is null && body.Planet is not null)
            return NotFound();

        location.System = system;
        location.Planet = planet;

        return unit.ToDTO();
    }


    [HttpGet("{userId}/units/{unitId}/location")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Returns more detailed information about the location a unit of user currently is about")]
    public ActionResult<UnitLocationDTO> GetUnitLocation(string userId, string unitId)
    {
        IUnit? unit = GetUnitFromRepository(userId, unitId);

        return unit is not null
            ? unit.ToLocationDTO()
            : NotFound();
    }


    private IUnit? GetUnitFromRepository(string userId, string unitId)
    {
        IUser? user = _usersRepository[userId];
        IUnit? unit = null;

        if (user is not null && user.Units.ContainsKey(unitId))
        {
            unit = user.Units[unitId];
        }

        return unit;
    }
}
