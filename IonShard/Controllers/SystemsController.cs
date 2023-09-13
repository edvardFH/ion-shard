using IonShard.Models.Map;
using IonShard.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IonShard.Controllers;

[Route("[controller]")]
[ApiController]
[Produces("application/json")]
public class SystemsController : ControllerBase
{
    private readonly MapRepository _map;


    public SystemsController(MapRepository mapBuilderService)
    {
        _map = mapBuilderService;
    }


    [HttpGet]
    public IEnumerable<StarSystem> GetAllSystems() => _map.Systems;


    [HttpGet("{systemName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<StarSystem> GetOneSystem(string systemName)
    {
        StarSystem? system = _map[systemName];

        return system is not null
            ? system
            : NotFound();
    }
    
    
    [HttpGet("{systemName}/planets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<Planet>> GetAllPlanetsFromSystem(string systemName)
    {
        IReadOnlyList<Planet>? planets = _map[systemName]?.Planets;

        return planets is not null 
            ? planets.ToList()
            : NotFound();
    }

    
    [HttpGet("{systemName}/planets/{planetName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Planet> GetOnePlanet(string systemName, string planetName)
    {
        Planet? planet = _map[systemName, planetName];

        return planet is not null
            ? planet
            : NotFound();
    }
}
