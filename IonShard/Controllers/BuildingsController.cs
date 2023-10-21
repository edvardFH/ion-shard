using IonShard.Contracts.DTO.Buildings;
using IonShard.Contracts.RequestBodies;
using IonShard.Domain.Units;
using IonShard.Domain.Users;
using IonShard.Mappers;
using IonShard.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IonShard.Controllers;

[Route("users")]
[ApiController]
[Produces("application/json")]
public class BuildingsController : ControllerBase
{
    private readonly UserRepository _usersRepository;
    private readonly MapRepository _mapRepository;
    private readonly BuildingRepository _buildingRepository;

    public BuildingsController(UserRepository usersRepository, MapRepository mapRepository, BuildingRepository buildingRepository)
    {
        _usersRepository = usersRepository;
        _mapRepository = mapRepository;
        _buildingRepository = buildingRepository;
    }


    [HttpPost]
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

        IUnit? unit = user.Units[body.BuilderId];

        if (unit is null || unit is not IBuilderUnit || unit.Location.Planet is null)
            return BadRequest();

        IBuilderUnit builder = (IBuilderUnit)unit;

        return builder.Build(body.Type).ToDTO();
    }
}
