using IonShard.Domain.Map;
using IonShard.Domain.Units;
using IonShard.Domain.Users;
using IonShard.DTO.Map;

namespace IonShard.Services
{

    public class UserFactory
    {
        private Random random = new Random();
        private MapRepository map;

        public UserFactory(MapRepository map)
        {
            this.map = map;
        }

        public User CreateNewUser(string id, string pseudo)
        {
            User newUser = new(id, pseudo, DateTime.Now);

            Unit userUnit = GetDefaultUnit();
            newUser.Units.Add(userUnit.Id, userUnit);

            return newUser;
        }

        private Unit GetDefaultUnit()
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
}
