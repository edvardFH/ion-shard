using IonShard.Domain.Users;
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


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> Get(string id)
        {
            UserDTO? user = _usersRepository[id]?.ToDTO();
            return user is not null
                ? user
                : NotFound();
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

   
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDTO?> Put(string id, [FromBody] CreateUserRequestBody body)
        {
            if (body.Id is not null && body.Pseudo is not null && body.Id == id)
            {
                User newUser = _userFactory.CreateNewUser(id, body.Pseudo);
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
