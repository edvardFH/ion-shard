using IonShard.Domain.Map;
using IonShard.Domain.Units;
using IonShard.Domain.Users;
using IonShard.Persistence.Repositories;

namespace IonShard.Services;


public class UserFactory : IUserFactory
{
    private Random _random = new Random();
    private MapRepository _map;
    private IUnitFactory _unitFactory;

    public UserFactory(MapRepository map, IUnitFactory unitFactory)
    {
        _map = map;
        _unitFactory = unitFactory;
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

        yield return _unitFactory.GetNewUnit(owner, starSystem, planet, "Scout");
        yield return _unitFactory.GetNewUnit(owner, starSystem, planet, "Builder");
    }

    private StarSystem GetRandomStarSystem() => _map.Systems[_random.Next(_map.Systems.Count)];

    private Planet? GetRandomPlanet(StarSystem starSystem)
    {
        Planet? randomPlanet = null;
        var returnANotNullPlanet = _random.Next(1) == 1;

        if (returnANotNullPlanet)
        {
            var starSystemSize = starSystem.Planets.Count();
            randomPlanet = starSystem.Planets[_random.Next(starSystemSize)];
        }

        return randomPlanet;
    }
}
