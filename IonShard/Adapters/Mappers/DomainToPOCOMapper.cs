using IonShard.Domain.Buildings;
using IonShard.Domain.Buildings.Mine;
using IonShard.Domain.Map;
using IonShard.Domain.Units;
using IonShard.Domain.Units.Cargo;
using IonShard.Domain.Users;
using IonShard.Persistence.POCOs;
using MongoDB.Bson;

namespace IonShard.Adapters.Mappers;

public static class DomainToPOCOMapper
{
    public static StarSystemPOCO ToPOCO(this StarSystem starSystem)
        => new StarSystemPOCO
            {
                InternalId = ObjectId.Empty,
                Name = starSystem.Name,
                Planets = starSystem.Planets
                    .Select(ToPOCO)
                    .ToList()
            };


    public static PlanetPOCO ToPOCO(this Planet planet)
        => new PlanetPOCO
            {
                Name = planet.Name,
                Size = planet.Size,
                ResourcesQuantity = planet.ResourcesQuantity
                    .ToDictionary
                    (
                        keyValuePair => keyValuePair.Key.Name,
                        keyValuePair => keyValuePair.Value
                    )
            };


    public static UserPOCO ToPOCO(this IUser user)
        => new UserPOCO
            {
                Id = user.Id,
                Pseudo = user.Pseudo,
                DateOfCreation = user.DateOfCreation,
                Units = user.Units.Values
                    .Select(ToPOCO)
                    .ToList(),
                Buildings = user.Buildings.Values
                    .Select(ToPOCO)
                    .ToList(),
                ResourcesQuantity = user.ResourcesQuantity
                    .ToDictionary
                    (
                        keyValuePair => keyValuePair.Key.Name,
                        keyValuePair => keyValuePair.Value
                    )
            };


    public static UnitPOCO ToPOCO(this IUnit unit)
        => new UnitPOCO
            {
                Id = unit.Id,
                Owner = unit.Owner.Id,
                System = unit.Location.System.Name,
                Planet = unit.Location.Planet?.Name,
                Type = unit.Type,
                HealthPoints = unit.HealthPoints,
                LoadedResources = unit is ICargoUnit cargo
                    ? cargo.LoadedResources.ToDictionary
                        (
                            keyValuePair => keyValuePair.Key.Name,
                            keyValuePair => keyValuePair.Value
                        )
                    : null
            };
    

    public static BuildingPOCO ToPOCO(this IBuilding building)
        => new BuildingPOCO
            {
                Id = building.Id,
                Type = building.Type,
                Builder = building.Builder.Id,
                System = building.Location.System.Name,
                Planet = building.Location.Planet?.Name
                    ?? throw new ArgumentException("Building must be on a planet."),
                IsBuilt = building.IsBuilt,
                EstimatedBuildTime = building.EstimatedBuildTime,
                ResourceCategory = building is IMineBuilding mine
                    ? mine.ResourceCategory.ToString()
                    : null
            };
}

