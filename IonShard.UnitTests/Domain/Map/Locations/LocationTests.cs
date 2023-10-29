using IonShard.Domain.Map;
using IonShard.Domain.Map.Locations;
using IonShard.UnitTests.Repository;

namespace IonShard.UnitTests.Domain.Map.Locations;

public class LocationTests
{
    private readonly LocalTestRepository _repository;
    private readonly StarSystem _sol;
    private readonly Planet _earth;
    private readonly ILocation _location;
    
    public LocationTests()
    {
        _repository = LocalTestRepository.GetInstance();
        _sol = _repository["sol"]!;
        _earth = _sol["earth"]!;
        _location = new Location(_sol, _earth);
    }
    
    [Fact]
    public void IsPlanetEntered_samePlanet()
    {
        bool result = _location.IsPlanetEntered(_earth);
        Assert.False(result);
    }
    
    [Fact]
    public void IsPlanetEntered_nullPlanet()
    {
        bool result = _location.IsPlanetEntered(null);
        Assert.False(result);
    }
    
    [Fact]
    public void IsPlanetEntered_differentPlanet()
    {
        bool result = _location.IsPlanetEntered(_sol["mars"]!);
        Assert.True(result);
    }
    
    [Fact]
    public void IsSystemChanged_sameSystem()
    {
        bool result = _location.IsSystemChanged(_sol);
        Assert.False(result);
    }
    
    [Fact]
    public void IsSystemChanged_differentSystem()
    {
        bool result = _location.IsSystemChanged(_repository["alpha-centauri"]!);
        Assert.True(result);
    }

    [Fact]
    public void IsPlanetLeft_samePlanet()
    {
        bool result = _location.IsPlanetLeft(_earth);
        Assert.False(result);
    }
    
    [Fact]
    public void IsPlanetLeft_nullPlanet()
    {
        bool result = _location.IsPlanetLeft(null);
        Assert.True(result);
    }
    
    [Fact]
    public void IsPlanetLeft_differentPlanet()
    {
        bool result = _location.IsPlanetLeft(_sol["mars"]!);
        Assert.True(result);
    }
}