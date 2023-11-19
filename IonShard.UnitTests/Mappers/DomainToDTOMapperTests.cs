using IonShard.Contracts.DTO.Buildings;
using IonShard.Contracts.DTO.Map;
using IonShard.Contracts.DTO.Units;
using IonShard.Contracts.DTO.Users;
using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using IonShard.Domain.Units;
using IonShard.Domain.Users;
using IonShard.UnitTests.Repository;
using IonShard.Mappers;

namespace IonShard.UnitTests.Mappers;

public class DomainToDtoMapperTests
{
    private readonly LocalTestRepository _repository;
    private readonly StarSystem _sol;
    private readonly Planet _earth;
    private readonly IUser _userJohn;

    public DomainToDtoMapperTests()
    {
        _repository = LocalTestRepository.GetInstance();
        _sol = _repository["sol"]!;
        _earth = _sol["earth"]!;
        _userJohn = _repository.UserRepository.Users["1"];
    }

    [Fact]
    public void StarSystem_ToDTO()
    {
        StarSystemDTO solDTO = _sol.ToDTO();

        Assert.NotNull(solDTO);
        Assert.Equal(_sol.Name, solDTO.name);
        Assert.Equal(_sol.Planets.Count, solDTO.Planets.Count);
        Assert.All(_sol.Planets, (planet,index) => planet.Name.Equals(solDTO.Planets[index].Name));
    }

    [Fact]
    public void Planet_ToDTO()
    {
        PlanetDTO earthDTO = _earth.ToDTO();

        Assert.NotNull(earthDTO);
        Assert.Equal(_earth.Name, earthDTO.Name);
        Assert.Equal(_earth.Size, earthDTO.Size);
    }

    [Fact]
    public void Location_withDetails_ToDTO()
    {
        ILocationWithDetails locationWithDetails = new LocationWithDetails(_sol, _earth);
        UnitLocationDTO locationWithDetailsDTO = locationWithDetails.ToDTO();

        Assert.NotNull(locationWithDetailsDTO);
        Assert.NotNull(locationWithDetailsDTO.ResourcesQuantity);
        Assert.Equal(locationWithDetails.Planet!.Name, locationWithDetailsDTO.Planet);
        Assert.Equal(locationWithDetails.System!.Name, locationWithDetailsDTO.System);
    }

    [Fact]
    public void Location_withoutDetails_ToDTO()
    {
        ILocation location = new Location(_sol, _earth);
        UnitLocationDTO locationDTO = location.ToDTO();

        Assert.NotNull(locationDTO);
        Assert.Null(locationDTO.ResourcesQuantity);
        Assert.Equal(location.Planet!.Name, locationDTO.Planet);
        Assert.Equal(location.System!.Name, locationDTO.System);
    }

    [Fact]
    public void User_ToDTO()
    {
        IUser user = new User("1", "john_doe", DateTime.Now);
        UserDTO userDTO = user.ToDTO();

        Assert.NotNull(userDTO);
        Assert.Equal(user.Id, userDTO.Id);
        Assert.Equal(user.Pseudo, userDTO.Pseudo);
    }

    [Fact]
    public void Building_ToDTO()
    {
        IBuilderUnit builderUnit = new BuilderUnit(_userJohn, _sol, _earth);
        IBuilding building = new MineBuilding(builderUnit, _sol, _earth);
        BuildingDTO buildingDTO = building.ToDTO();

        Assert.NotNull(buildingDTO);
        Assert.Equal(building.Id, buildingDTO.Id);
        Assert.Equal(building.Location.Planet!.Name, buildingDTO.Planet);
        Assert.Equal(building.Location.System.Name, buildingDTO.System);
        Assert.Equal(building.Type, buildingDTO.Type);
    }

    [Fact]
    public void Unit_ToDTO()
    {
        IUnit builderUnit = new BuilderUnit(_userJohn, _sol, _earth);
        UnitDTO builderUnitDTO = builderUnit.ToDTO();

        Assert.NotNull(builderUnitDTO);
        Assert.Equal(builderUnit.Id, builderUnitDTO.Id);
        Assert.Equal(builderUnit.Location.System.Name, builderUnitDTO.System);
        Assert.Equal(builderUnit.Location.Planet!.Name, builderUnitDTO.Planet);
        Assert.NotNull(builderUnitDTO.DestinationSystem);
        Assert.NotNull(builderUnitDTO.DestinationPlanet);
    }
}