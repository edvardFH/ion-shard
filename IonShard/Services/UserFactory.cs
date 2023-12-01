using IonShard.Domain.Map;
using IonShard.Domain.Units;
using IonShard.Domain.Units.Builder;
using IonShard.Domain.Units.Scout;
using IonShard.Domain.Users;
using IonShard.Persistence.Repositories;

namespace IonShard.Services;


public class UserFactory
{
    private Random random = new Random();
    private MapRepository map;

    public UserFactory(MapRepository map)
    {
        this.map = map;
    }

    public IUser GetNewUser(string id, string pseudo)
    {
        IUser newUser = new User(id, pseudo, DateTime.Now);

        GetDefaultUnits(newUser)
            .ToList()
            .ForEach(unit => newUser.AddUnit(unit));

        return newUser;
    }

    private IEnumerable<IUnit> GetDefaultUnits(IUser owner)
    {
        StarSystem starSystem = GetRandomStarSystem();
        Planet? planet = GetRandomPlanet(starSystem);

        yield return new ScoutUnit(owner, starSystem, planet);
        yield return new BuilderUnit(owner, starSystem, planet);
    }

    private StarSystem GetRandomStarSystem() => map.Systems[random.Next(map.Systems.Count)];

    private Planet? GetRandomPlanet(StarSystem starSystem)
    {
        Planet? randomPlanet = null;
        var returnANotNullPlanet = random.Next(1) == 1;

        if (returnANotNullPlanet)
        {
            var starSystemSize = starSystem.Planets.Count();
            randomPlanet = starSystem.Planets[random.Next(starSystemSize)];
        }

        return randomPlanet;
    }
}
