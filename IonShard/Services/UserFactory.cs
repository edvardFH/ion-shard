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

        ScoutUnit userUnit = GetDefaultUnit();
        newUser.Units.Add(userUnit.Id, userUnit);

        return newUser;
    }

    private ScoutUnit GetDefaultUnit()
    {
        StarSystem starSystem = GetRandomStarSystem();
        return new(random.NextGuid().ToString(), starSystem, GetRandomPlanet(starSystem));
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
