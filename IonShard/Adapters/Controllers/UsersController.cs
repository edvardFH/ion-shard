using IonShard.Application;
using IonShard.Contracts.DTO.Users;
using IonShard.Contracts.RequestBodies;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Users;
using IonShard.Mappers;
using IonShard.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.RegularExpressions;


namespace IonShard.Controllers;

[Route("[controller]")]
[ApiController]
[Produces("application/json")]
public class UsersController : ControllerBase
{

    private readonly UserRepository _usersRepository;
    private readonly IUserFactory _userFactory;
    private readonly IResourceFactory _resourceFactory;

    public UsersController
        (
            UserRepository usersRepository,
            IUserFactory userFactory,
            IResourceFactory resourceFactory
        )
    {
        _usersRepository = usersRepository;
        _userFactory = userFactory;
        _resourceFactory = resourceFactory;
    }


    [HttpPut("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Create a new user")]
    public ActionResult<UserDTO?> CreateNewUser(string userId, [FromBody] UserPutRequestBody body)
    {
        if (body is not { Id: string, Pseudo: string }
         || body.Id != userId
         || !Regex.IsMatch(userId, "^[a-zA-Z0-9_-]+$"))
            return BadRequest();

        if (_usersRepository.Users.ContainsKey(userId))
        {
            var user = _usersRepository[userId];
            if (HttpContext.User.IsInRole("Admin") && body.ResourcesQuantity is not null)
            {
                try
                {
                    user?.UpdateResources(
                        _resourceFactory.TryParseToResourceQuantity(body.ResourcesQuantity).AsReadOnly());
                }
                catch (ArgumentException)
                {
                    return BadRequest();
                }
            }
            return user?.ToDTO();
        }


        IUser newUser;

        if (HttpContext.User.IsInRole("Shard") && body.DateOfCreation is DateTime dateOfCreation)
            newUser = _userFactory.CreateUser
                (
                    userId,
                    body.Pseudo,
                    dateOfCreation,
                    new Dictionary<IResource, int>()
                );
        else
            newUser = _userFactory.CreateNewUser(userId, body.Pseudo);

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
