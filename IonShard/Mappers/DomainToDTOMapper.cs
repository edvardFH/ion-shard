using IonShard.Contracts.DTO.Buildings;
using IonShard.Contracts.DTO.Map;
using IonShard.Contracts.DTO.Units;
using IonShard.Contracts.DTO.Users;
using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Units;
using IonShard.Domain.Users;

namespace IonShard.Mappers;

public static class DomainToDTOMapper
{
    public static BuildingDTO ToDTO(this IBuilding building)
        => new BuildingDTO(
            building.Id,
            building.Type,
            building.Location.System.Name,
            building.Location.Planet?.Name);


    public static UserDTO ToDTO(this IUser user)
        => new UserDTO(user.Id, user.Pseudo, user.DateOfCreation);


    public static UnitDTO ToDTO(this IUnit unit)
        => new UnitDTO(
            unit.Id,
            unit.Type,
            unit.Location.System.Name,
            unit.Location.Planet?.Name,
            null,
            null,
            null);


    public static UnitLocationDTO ToLocationDTO(this ScoutUnit unit) 
        => new UnitLocationDTO(
            unit.Location.System.Name,
            unit.Location.Planet?.Name,
            unit.Location.Planet?.ResourcesQuantity
                .ToDictionary(
                    resource => resource.Key.ToString().ToLower(),
                    resource => resource.Value));

    public static UnitLocationDTO ToLocationDTO(this IUnit unit)
        => new UnitLocationDTO(
            unit.Location.System.Name,
            unit.Location.Planet?.Name,
            null);



    public static PlanetDTO ToDTO(this Planet planet)
        => new PlanetDTO(planet.Name, planet.Size);


    public static StarSystemDTO ToDTO(this StarSystem starSystem)
        => new StarSystemDTO(
            starSystem.Name,
            starSystem.Planets.ToList().ConvertAll(planet => planet.ToDTO()));
}
