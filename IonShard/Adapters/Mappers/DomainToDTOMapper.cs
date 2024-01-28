using IonShard.Contracts.DTO.Buildings;
using IonShard.Contracts.DTO.Map;
using IonShard.Contracts.DTO.Units;
using IonShard.Contracts.DTO.Users;
using IonShard.Domain.Buildings;
using IonShard.Domain.Buildings.Mine;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units;
using IonShard.Domain.Units.Cargo;
using IonShard.Domain.Users;

namespace IonShard.Mappers;

public static class DomainToDTOMapper
{
    public static BuildingDTO ToDTO(this IBuilding building)
    {
        ResourceCategory? resourceCategory = building switch
        {
            IMineBuilding mineBuilding => mineBuilding.ResourceCategory,
            _ => null,
        };

        return new BuildingDTO(
            building.Id,
            building.Type.ToLower(),
            building.Location.System.Name,
            building.Location.Planet?.Name,
            building.IsBuilt,
            building.EstimatedBuildTime,
            resourceCategory);
    }


    public static UserDTO ToDTO(this IUser user)
        => new UserDTO(user.Id, user.Pseudo, user.DateOfCreation,
            user.ResourcesQuantity.ToDictionary(
                resource => resource.Key.Name.ToString().ToLower(),
                resource => resource.Value));


    public static UnitDTO ToDTO(this IUnit unit)
    {
        var unitSystem = unit.Location.System.Name;
        var unitPlanet = unit.Location.Planet?.Name;

        return new UnitDTO(
            unit.Id,
            unit.Type.ToLower(),
            unitSystem,
            unitPlanet,
            unit.Destination?.System.Name ?? unitSystem,
            unit.Destination?.Planet?.Name ?? unitPlanet,
            unit.Destination?.EstimatedTimeOfArrival.ToString(),
            unit.HealthPoints,
            unit is ICargoUnit cargoUnit
                ? cargoUnit.LoadedResources.ToDictionary(
                    resource => resource.Key.Name.ToString().ToLower(),
                    resource => resource.Value)
                : new Dictionary<string, int>());
    }


    public static UnitLocationDTO ToDTO(this ILocation location)
        => location switch
        {
            ILocationWithDetails locationWithDetails => locationWithDetails.ToDTO(),
            _ => location.ToDTOWithoutDetails()
        };

    private static UnitLocationDTO ToDTOWithoutDetails(this ILocation location)
        => new UnitLocationDTO(
            location.System.Name,
            location.Planet?.Name,
            null);

    public static UnitLocationDTO ToDTO(this ILocationWithDetails location)
        => new UnitLocationDTO(
            location.System.Name,
            location.Planet?.Name,
            location.Planet?.ResourcesQuantity
                .ToDictionary(
                    resource => resource.Key.Name.ToString().ToLower(),
                    resource => resource.Value));


    public static PlanetDTO ToDTO(this Planet planet)
        => new PlanetDTO(planet.Name, planet.Size);


    public static StarSystemDTO ToDTO(this StarSystem starSystem)
        => new StarSystemDTO(
            starSystem.Name,
            starSystem.Planets.ToList().ConvertAll(planet => planet.ToDTO()));
}
