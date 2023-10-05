using IonShard.Domain.Map;
using IonShard.Domain.Units;
using IonShard.Domain.Users;
using IonShard.DTO.Units;
using IonShard.DTO.Users;
using IonShard.Mappers;
using IonShard.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.RegularExpressions;


namespace IonShard.Controllers;

[Route("[controller]")]
[ApiController]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    
    private readonly UsersRepository _usersRepository;
    private readonly UserFactory _userFactory;
    private readonly MapRepository _mapRepository;

    public UsersController(UsersRepository usersRepository, UserFactory userFactory, MapRepository mapRepository)
    {
        _usersRepository = usersRepository;
        _userFactory = userFactory;
        _mapRepository = mapRepository;
    }


    [HttpPut("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Create a new user")]
    public ActionResult<UserDTO?> CreateNewUser(string userId, [FromBody] CreateUserPutRequestBody body)
    {
        if (body.Id is not null && body.Pseudo is not null && body.Id == userId
            && Regex.IsMatch(userId, "^[a-zA-Z0-9_-]+$"))
        {
            User newUser = _userFactory.CreateNewUser(userId, body.Pseudo);
            _usersRepository.Users.Add(newUser.Id, newUser);
            return newUser.ToDTO();
        }
        else
        {
            return BadRequest();
        }
    }


    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Returns details of an existing user")]
    public ActionResult<UserDTO> GetUser(string userId)
    {
        UserDTO? user = _usersRepository[userId]?.ToDTO();

        return user is not null
            ? user
            : NotFound();
    }


    [HttpGet("{userId}/units")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Returns all units of a user")]
    public ActionResult<IEnumerable<UnitDTO>> GetAllUnitsOfUser(string userId)
    {
        User? user = _usersRepository[userId];

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
    [SwaggerOperation(Summary = "Returns information about one unit")]
    public ActionResult<UnitDTO> GetOneUnitFromUser(string userId, string unitId)
    {
        Unit? unit = GetUnitFromRepository(userId, unitId);

        return unit is not null
            ? unit.ToDTO()
            : NotFound();
    }


    [HttpPut("{userId}/units/{unitId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UnitDTO?> MoveUnitOfUser(string userId, string unitId, [FromBody] MoveUnitPutRequestBody body)
    {
        if (unitId != body.Id || body.Id is null || body.System is null)
            return BadRequest();

        Unit? unit = GetUnitFromRepository(userId, unitId);
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
    public ActionResult<UnitLocationDTO> GetUnitLocation(string userId, string unitId)
    {
        Unit? unit = GetUnitFromRepository(userId, unitId);
        var location = unit?.ToLocationDTO();

        return unit is not null
            ? unit.ToLocationDTO()
            : NotFound();
    }


    private Unit? GetUnitFromRepository(string userId, string unitId)
    {
        User? user = _usersRepository[userId];
        Unit? unit = null;

        if (user is not null && user.Units.ContainsKey(unitId))
        {
            unit = user.Units[unitId];
        }

        return unit;
    }
}
