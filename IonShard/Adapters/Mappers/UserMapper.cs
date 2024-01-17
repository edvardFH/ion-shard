using IonShard.Application;
using IonShard.Domain.Buildings;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units;
using IonShard.Domain.Units.Builder;
using IonShard.Domain.Users;
using IonShard.Persistence.POCOs;
using IonShard.Persistence.Repositories;

namespace IonShard.Adapters.Mappers;

public class UserMapper
{
    private readonly IUserFactory _userFactory;
    private readonly IUnitFactory _unitFactory;
    private readonly IBuildingFactory _buildingFactory;
    private readonly IResourceFactory _resourceFactory;
    private readonly MapRepository _mapRepository;


    public UserMapper
        (
            IUserFactory userFactory,
            IUnitFactory unitFactory,
            IBuildingFactory buildingFactory,
            IResourceFactory resourceFactory,
            MapRepository mapRepository
        )
    {
        _userFactory = userFactory;
        _unitFactory = unitFactory;
        _buildingFactory = buildingFactory;
        _resourceFactory = resourceFactory;
        _mapRepository = mapRepository;
    }


    public IUser UserPOCOToDomain(UserPOCO user)
    {
        IUser domainUser = _userFactory.CreateUser(
            user.Id,
            user.Pseudo,
            user.DateOfCreation,
            _resourceFactory
                .TryParseToResourceQuantity(user.ResourcesQuantity)
                .AsReadOnly()
        );

        user.Units
            .ToList()
            .ForEach(unit => domainUser.AddUnit(UnitPOCOToDomain(unit, domainUser)));

        user.Buildings
            .ToList()
            .ForEach(building => domainUser.AddBuilding(BuildingPOCOToDomain(building, domainUser)));

        return domainUser;
    }


    private IUnit UnitPOCOToDomain(UnitPOCO unit, IUser owner)
    {
        var system = _mapRepository[unit.System]
            ?? throw new ArgumentException("System does not exist");

        var planet = system[unit.Planet ?? ""];
        
        return _unitFactory.CreateUnitWithId(
            unit.Id,
            owner,
            system,
            planet,
            unit.Type,
            _buildingFactory,
            unit.HealthPoints,
            unit.LoadedResources is not null
                ? _resourceFactory
                    .TryParseToResourceQuantity(unit.LoadedResources)
                    .AsReadOnly()
                : null
        );
    }


    private IBuilding BuildingPOCOToDomain(BuildingPOCO building, IUser owner)
    {
        var system = _mapRepository[building.System]
            ?? throw new ArgumentException("System does not exist");

        var planet = system[building.Planet ?? ""] 
            ?? throw new ArgumentException("Building must be on a planet"); ;

        var unit = owner.Units[building.Builder];

        if (unit is not IBuilderUnit builder)
            throw new ArgumentException("Building must have been built by a builder");

        if (!Enum.TryParse<ResourceCategory>(building.ResourceCategory, out var resourceCategory))
            throw new ArgumentException($"Invalid resource category: {building.ResourceCategory}");

        return _buildingFactory.CreateBuilding(
            building.Type,
            builder,
            system,
            planet,
            building.IsBuilt,
            building.EstimatedBuildTime,
           resourceCategory
        );
    }
}