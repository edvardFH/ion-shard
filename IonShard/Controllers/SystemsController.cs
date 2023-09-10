using IonShard.Models.Space;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IonShard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SystemsController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<StarSystem> Get()
        {
            return new List<StarSystem>();
        }

       
        [HttpGet("{systemName}")]
        public ActionResult<StarSystem> GetSystem(string systemName)
        {
            return systemName.Equals("Solar System")
                ? new StarSystem("Solar System", new List<Planet>())
                : NotFound();
        }
        
        
        [HttpGet("{systemName}/planets")]
        public IEnumerable<Planet> GetPlanets(string systemName)
        {
            return new List<Planet>();
        }
        
        [HttpGet("{systemName}/planets/{planetName}")]
        public Planet GetPlanet(string systemName, string planetName)
        {
            return new Planet("Mars", 1200);
        }
    }
}
