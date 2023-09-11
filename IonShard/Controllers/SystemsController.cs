using IonShard.Models.Space;
using IonShard.Services;
using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IonShard.Controllers;

[Route("[controller]")]
[ApiController]
[Produces("application/json")]
public class SystemsController : ControllerBase
{
    private MapBuilderService _mapBuilderService;

    public SystemsController(MapBuilderService mapBuilderService)
    {
        _mapBuilderService = mapBuilderService;
    }

    [HttpGet]
    public IEnumerable<StarSystem> GetSystems() => _mapBuilderService.GetAllSystems();


    [HttpGet("{systemName}")]
    public ActionResult<StarSystem> GetSystem(string systemName)
    {
        var system = _mapBuilderService.GetSystem(systemName);
        return system != null ? system : NotFound();
    }
    
    
    [HttpGet("{systemName}/planets")]
    public ActionResult<IEnumerable<Planet>> GetPlanets(string systemName)
    {
        var planets = _mapBuilderService.GetPlanets(systemName);
        return planets != null ? planets : NotFound();
    }
    
    [HttpGet("{systemName}/planets/{planetName}")]
    public ActionResult<Planet> GetPlanet(string systemName, string planetName)
    {
        var planet = _mapBuilderService.GetPlanet(systemName, planetName);
        return planet != null ? planet : NotFound();
    }
}
