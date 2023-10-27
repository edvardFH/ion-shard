using IonShard.Contracts.DTO.Buildings;
using IonShard.Contracts.RequestBodies;
using IonShard.Domain.Buildings;
using IonShard.Domain.Units;
using IonShard.Domain.Users;
using IonShard.Mappers;
using IonShard.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IonShard.Controllers;

[Route("Users")]
[ApiController]
[Produces("application/json")]
public class BuildingsController : ControllerBase
{
    private readonly UserRepository _usersRepository;

    public BuildingsController(UserRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }


    [HttpPost("{userId}/buildings")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<BuildingDTO> Post(string userId, [FromBody] CreateBuildingPostRequestBody body)
    {
        IUser? user = _usersRepository[userId];

        if(user is null)
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
        user.Buildings.Add(building.Id, building);

        return builder.Build(body.Type).ToDTO();
    }
}
