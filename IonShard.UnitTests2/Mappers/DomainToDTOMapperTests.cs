using IonShard.Contracts.DTO.Map;
using IonShard.Domain.Map;
using IonShard.UnitTests.Repository;
using IonShard.Mappers;

namespace IonShard.UnitTests.Mappers;

public class DomainToDtoMapperTests
{
    private readonly LocalTestRepository _repository = LocalTestRepository.GetInstance();

    [Fact]
    public void StarSystem_ToDTO()
    {
        StarSystem sol = _repository["sol"]!;
        StarSystemDTO solDTO = sol.ToDTO();
        
        Assert.NotNull(solDTO);
        Assert.Equal(sol.Name, solDTO.Name);
        Assert.Equal(sol.Planets.Count, solDTO.Planets.Count);
        Assert.All(sol.Planets, (planet,index) => planet.Name.Equals(solDTO.Planets[index].Name));
    }
    
    [Fact]
    public void Planet_ToDTO()
    {
        Planet earth = _repository["sol"]!["earth"]!;
        PlanetDTO earthDTO = earth.ToDTO();
        
        Assert.NotNull(earthDTO);
        Assert.Equal(earth.Name, earthDTO.Name);
        Assert.Equal(earth.Size, earthDTO.Size);
    }
}