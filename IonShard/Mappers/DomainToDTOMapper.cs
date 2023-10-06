using IonShard.Contracts.DTO.Map;
using IonShard.Contracts.DTO.Units;
using IonShard.Contracts.DTO.Users;
using IonShard.Domain.Map;
using IonShard.Domain.Units;
using IonShard.Domain.Users;

namespace IonShard.Mappers;

public static class DomainToDTOMapper
{
    public static UserDTO ToDTO(this User user)
        => new UserDTO(user.Id, user.Pseudo, user.DateOfCreation);


    public static UnitDTO ToDTO(this Unit unit)
    {
        var unitLocation = unit.Location;

        return new UnitDTO(
            unit.Id,
            unitLocation.System.Name,
            unitLocation.Planet?.Name);
    }


    public static UnitLocationDTO ToLocationDTO(this Unit unit) 
        => new UnitLocationDTO(
            unit.Location.System.Name,
            unit.Location.Planet?.Name,
            unit.Location.Planet?.ResourcesQuantity
                .ToDictionary(
                    resource => resource.Key.ToString().ToLower(),
                    resource => resource.Value));
   


    public static PlanetDTO ToDTO(this Planet planet)
        => new PlanetDTO(planet.Name, planet.Size);


    public static StarSystemDTO ToDTO(this StarSystem starSystem)
        => new StarSystemDTO(
            starSystem.Name,
            starSystem.Planets.ToList().ConvertAll(planet => planet.ToDTO()));
}
