using IonShard.Domain.Map;
using IonShard.Domain.Units;
using IonShard.Domain.Users;
using IonShard.Persistence.Repositories;
using IonShard.Utils;

namespace IonShard.Services;


public class UserFactory
{
    private Random random = new Random();
    private MapRepository map;

    public UserFactory(MapRepository map)
    {
        this.map = map;
    }

    public IUser CreateNewUser(string id, string pseudo)
    {
        IUser newUser = new User(id, pseudo, DateTime.Now);

        GetDefaultUnits()
            .ToList()
            .ForEach(unit => newUser.Units.Add(unit.Id, unit));

        return newUser;
    }

    private IEnumerable<IUnit> GetDefaultUnits()
    {
        StarSystem starSystem = GetRandomStarSystem();
        Planet? planet = GetRandomPlanet(starSystem);

        yield return new ScoutUnit(starSystem, planet);
        yield return new BuilderUnit(starSystem, planet);
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
