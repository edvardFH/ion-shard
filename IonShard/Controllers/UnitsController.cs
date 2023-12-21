using IonShard.Configuration.Wormholes;
using IonShard.Contracts.DTO.Units;
using IonShard.Contracts.RequestBodies;
using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units;
using IonShard.Domain.Units.Cargo;
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
    private readonly IResourceFactory _resourceFactory;
    private readonly IWormholesService _wormholesService;

    public UnitsController
        (
            UserRepository usersRepository,
            MapRepository mapRepository,
            IClock clock,
            IConfiguration configuration,
            IUnitFactory unitFactory,
            IBuildingFactory buildingFactory,
            IResourceFactory resourceFactory,
            IWormholesService wormholesService
        )
    {
        _usersRepository = usersRepository;
        _mapRepository = mapRepository;
        _clock = clock;
        MaximumWaitingTimeBeforeResponse = TimeSpan.FromSeconds(
            configuration.GetValue<int>(
                "Controllers:MaximumWaitingTimeBeforeResponse"));
        _unitFactory = unitFactory;
        _buildingFactory = buildingFactory;
        _resourceFactory = resourceFactory;
        _wormholesService = wormholesService;
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
        if (unitId != body.Id || body.Id is null)
            return BadRequest();

        IUnit? unit = GetUnitFromRepository(userId, unitId);

        if (unit is null)
        {
            if (body.Type is null || !_unitFactory.DoesTypeExist(body.Type))
                return BadRequest();

            IUser? user = _usersRepository[userId];

            if (user is null)
                return NotFound();

            if (HttpContext.User.IsInRole("Admin"))
                return HandleAdminRequest(user, body);
            else if (HttpContext.User.IsInRole("Shard"))
                return HandleShardRequest(user, body);
            else
                return Unauthorized();
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

        if (unit is not ICargoUnit cargo)
            return body.ResourcesQuantity is null || body.ResourcesQuantity.Count == 0
                ? unit.ToDTO()
                : BadRequest(body.ResourcesQuantity);

        if (body.ResourcesQuantity is null)
            return BadRequest();

        try
        {
            var resourcesQuantity = _resourceFactory.TryParseToResourceQuantity(body.ResourcesQuantity);

            if (SameQuantity(resourcesQuantity.AsReadOnly(), cargo.LoadedResources))
                return cargo.ToDTO();

            LoadCargo(cargo, resourcesQuantity.AsReadOnly());
        }
        catch (Exception exception)
        {
            return BadRequest(exception.ToString()); // TODO: remove exception from response
        }

        return cargo.ToDTO();
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


    private ActionResult<UnitDTO?> HandleAdminRequest(IUser user, MoveUnitPutRequestBody body)
    {
        if (body.System is null)
            return BadRequest();

        StarSystem? starSystem = _mapRepository[body.System];

        if (starSystem is null)
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

    private ActionResult<UnitDTO?> HandleShardRequest(IUser user, MoveUnitPutRequestBody body)
    {
        var shardName = HttpContext.User.Identities.First().Name ?? "";
        var starSystemName = _wormholesService[shardName]?.System ?? "";
        var starSystem = _mapRepository[starSystemName];

        if (body.Type is null || starSystem is null)
            return BadRequest();

        Planet? planet = body.Planet is null
            ? null
            : _mapRepository[starSystem.Name, body.Planet];

        IReadOnlyDictionary<IResource, int>? loadedResources = null;

        if (body.Type is "cargo")
        {
            if (body.ResourcesQuantity is null)
                return BadRequest();
            else
                try
                {
                    loadedResources = _resourceFactory.TryParseToResourceQuantity(body.ResourcesQuantity).AsReadOnly();
                }
                catch
                {
                    return BadRequest();
                }
        }

        // TODO: handle hp for cargo (wainting for Mr Pineau answer)

        return _unitFactory.CreateUnitWithId
            (
                body.Id,
                user,
                starSystem,
                planet,
                body.Type,
                _buildingFactory,
                body.Health,
                loadedResources
            ).ToDTO();
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


    private bool SameQuantity(
            IReadOnlyDictionary<IResource, int> resourcesQuantityA,
            IReadOnlyDictionary<IResource, int> resourcesQuantityB)
        => resourcesQuantityA.All(resourcesQuantityB.Contains) && resourcesQuantityA.Count == resourcesQuantityB.Count;


    private void LoadCargo(ICargoUnit cargo, IReadOnlyDictionary<IResource, int> resourcesQuantity)
    {
        resourcesQuantity.ToList().ForEach(resource =>
        {
            var cargoResource = cargo.LoadedResources.ContainsKey(resource.Key)
                ? cargo.LoadedResources[resource.Key]
                : 0;

            var difference = resource.Value - cargoResource;

            if (difference < 0)
                cargo.Unload(resource.Key, -difference);
            else
                cargo.Load(resource.Key, difference);
        });
    }
}
