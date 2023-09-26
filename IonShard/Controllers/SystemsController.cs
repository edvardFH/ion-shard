using IonShard.DTO.Map;
using IonShard.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IonShard.Controllers;

[Route("[controller]")]
[ApiController]
[Produces("application/json")]
public class SystemsController : ControllerBase
{
    private readonly UniverseDTO _universe;


    public SystemsController(MapRepository mapBuilderService)
    {
        _universe = new UniverseDTO(mapBuilderService.Systems);
    }


    [HttpGet]
    [SwaggerOperation(Summary="Fetches all systems")]
    public IEnumerable<StarSystemDTO> GetAllSystems() => _universe.Systems;


    [HttpGet("{systemName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Fetches a specific system")]
    public ActionResult<StarSystemDTO> GetOneSystem(string systemName)
    {
        StarSystemDTO? system = _universe[systemName];

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
        IReadOnlyList<PlanetDTO>? planets = _universe[systemName]?.Planets;

        return planets is not null 
            ? planets.ToList()
            : NotFound();
    }

    
    [HttpGet("{systemName}/planets/{planetName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Fetches a single planet")]
    public ActionResult<PlanetDTO> GetOnePlanet(string systemName, string planetName)
    {
        PlanetDTO? planet = _universe[systemName]?[planetName];

        return planet is not null
            ? planet
            : NotFound();
    }
}
