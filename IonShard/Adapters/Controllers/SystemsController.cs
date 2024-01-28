using IonShard.Contracts.DTO.Map;
using IonShard.Mappers;
using IonShard.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace IonShard.Adapters.Controllers;

[Route("[controller]")]
[ApiController]
[Produces("application/json")]
public class SystemsController : ControllerBase
{
    private readonly MapRepository _map;


    public SystemsController(MapRepository mapRepository)
    {
        _map = mapRepository;
    }


    [HttpGet]
    [SwaggerOperation(Summary = "Fetches all systems, and their planets")]
    public IEnumerable<StarSystemDTO> GetAllSystems()
        => _map.Systems
            .ToList()
            .ConvertAll(system => system.ToDTO());


    [HttpGet("{systemName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Fetches a single system, and all its planets")]
    public ActionResult<StarSystemDTO> GetOneSystem(string systemName)
    {
        StarSystemDTO? system = _map[systemName]?.ToDTO();

        return system is not null
            ? system
            : NotFound();
    }


    [HttpGet("{systemName}/planets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Fetches all planet for a specific system")]
    public ActionResult<IEnumerable<PlanetDTO>> GetAllPlanetsFromSystem(string systemName)
    {
        IReadOnlyList<PlanetDTO>? planets =
            _map[systemName]?.Planets
                .ToList()
                .ConvertAll(planet => planet.ToDTO());

        return planets is not null
            ? planets.ToList()
            : NotFound();
    }


    [HttpGet("{systemName}/planets/{planetName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Fetches a single planet of a system")]
    public ActionResult<PlanetDTO> GetOnePlanet(string systemName, string planetName)
    {
        PlanetDTO? planet = _map[systemName, planetName]?.ToDTO();

        return planet is not null
            ? planet
            : NotFound();
    }
}
