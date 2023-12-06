using IonShard.Configuration;
using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Units;
using IonShard.Domain.Users;
using IonShard.Persistence.Repositories;

namespace IonShard.Services;


public class UserFactory : IUserFactory
{
    private readonly Random _random = new Random();
    private readonly MapRepository _map;
    private readonly IUnitFactory _unitFactory;
    private readonly IBuildingFactory _buildingFactory;
    private readonly IGameRulesService _gameRulesService;

    public UserFactory
        (
            MapRepository map,
            IUnitFactory unitFactory,
            IBuildingFactory buildingFactory,
            IGameRulesService gameRuleService
        )
    {
        _map = map;
        _unitFactory = unitFactory;
        _buildingFactory = buildingFactory;
        _gameRulesService = gameRuleService;
    }

    public IUser CreateUser(string id, string pseudo)
    {
        IUser newUser = new User(id, pseudo, DateTime.Now, _gameRulesService);
        AddDefaultUnitsToUser(newUser);

        return newUser;
    }

    private void AddDefaultUnitsToUser(IUser owner)
    {
        StarSystem starSystem = GetRandomStarSystem();
        Planet? planet = GetRandomPlanet(starSystem);

        _unitFactory.CreateUnit(owner, starSystem, planet, "Scout", null);
        _unitFactory.CreateUnit(owner, starSystem, planet, "Builder", _buildingFactory);
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
