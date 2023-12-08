using IonShard.Contracts.DTO.Users;
using IonShard.Contracts.RequestBodies;
using IonShard.Domain.Users;
using IonShard.Mappers;
using IonShard.Persistence.Repositories;
using IonShard.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.RegularExpressions;
using IonShard.Configuration.Auth;


namespace IonShard.Controllers;

[Route("[controller]")]
[ApiController]
[Produces("application/json")]
public class UsersController : ControllerBase
{

    private readonly UserRepository _usersRepository;
    private readonly IUserFactory _userFactory;

    public UsersController
        (
            UserRepository usersRepository,
            IUserFactory userFactory
        )
    {
        _usersRepository = usersRepository;
        _userFactory = userFactory;
    }


    [HttpPut("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Create a new user")]
    public ActionResult<UserDTO?> CreateNewUser(string userId, [FromBody] CreateUserPutRequestBody body)
    {
        if (body is not { Id: string, Pseudo: string }
         || body.Id != userId
         || !Regex.IsMatch(userId, "^[a-zA-Z0-9_-]+$"))
            return BadRequest();

        if (_usersRepository.Users.ContainsKey(userId))
        {
            if (HttpContext.User.IsInRole("Admin")) 
                return BadRequest("Admin user : WIP functionality"); // TODO: handle admin request
            return BadRequest("Non-admin user : WIP functionality"); // TODO: handle admin request
        }

        IUser newUser = _userFactory.CreateUser(userId, body.Pseudo);
        _usersRepository.Users.Add(newUser.Id, newUser);

        return newUser.ToDTO();
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
}
