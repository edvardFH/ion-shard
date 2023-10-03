using IonShard.Domain.Units;
using IonShard.Domain.Users;
using IonShard.DTO.Units;
using IonShard.DTO.Users;
using IonShard.Mappers;
using IonShard.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IonShard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        
        private readonly UsersRepository _usersRepository;
        private readonly UserFactory _userFactory;

        public UsersController(UsersRepository usersRepository, UserFactory userFactory)
        {
            _usersRepository = usersRepository;
            _userFactory = userFactory;
        }


        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        public ActionResult<UnitDTO> GetOneUnitFromUser(string userId, string unitId)
        {
            User? user = _usersRepository[userId];
            Unit? unit = null;

            if (user is not null && user.Units.ContainsKey(userId))
            {
                unit = user.Units[userId];
            }

            return unit is not null
                ? unit.ToDTO()
                : NotFound();
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

   
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDTO?> CreateNewUser(string userId, [FromBody] CreateUserRequestBody body)
        {
            if (body.Id is not null && body.Pseudo is not null && body.Id == userId)
            {
                User newUser = _userFactory.CreateNewUser(userId, body.Pseudo);
                _usersRepository.Users.Add(newUser.Id, newUser);
                return newUser.ToDTO();
            } else
            {
                return NotFound();
            }
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
