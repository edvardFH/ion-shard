using IonShard.Configuration.Gamerules;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units;
using IonShard.Domain.Units.Builder;
using IonShard.Domain.Users;
using IonShard.UnitTests.Repository;
using Moq;
using Shard.Shared.Web.IntegrationTests.Clock;

namespace IonShard.UnitTests.Domain.Units;

public class BuilderUnitTests
{
    private readonly StarSystem _sol;
    private readonly Planet _earth;
    private readonly FakeClock _clock;
    private readonly IUnit _builderUnit;

    public BuilderUnitTests()
    {
        var mockResourceFactory = new Mock<IResourceFactory>();
        var mockGameRulesService = new Mock<IGameRulesService>();
        _earth = new Planet("earth", 12742, ResourcesTestProvider.GetResources());
        _sol = new StarSystem("sol", new [] { _earth });
        var userJohn = new User("1", "john.doe", DateTime.Now, mockGameRulesService.Object, mockResourceFactory.Object);
        _clock = new FakeClock();
        _builderUnit = new BuilderUnit("id1", null, userJohn, _sol, _earth, null, 100, _clock);
    }

    [Fact]
    public async Task MoveUnit()
    {
        var alphaCentauri = new StarSystem("alpha-centauri", new List<Planet>());
        _builderUnit.StartTravel(_clock, alphaCentauri, null);
        Assert.Equal(_sol, _builderUnit.Location.System);
        Assert.Equal(alphaCentauri, _builderUnit.Destination!.System);
        
        await _clock.Advance(TimeSpan.FromMinutes(1));
        
        Assert.Equal(alphaCentauri, _builderUnit.Location.System);
        Assert.Null(_builderUnit.Location.Planet);
    }
    
    [Fact]
    public async Task CancelUnitTravel()
    {
        var alphaCentauri = new StarSystem("alpha-centauri", new List<Planet>());
        _builderUnit.StartTravel(_clock, alphaCentauri, null);
        
        await _clock.Advance(TimeSpan.FromSeconds(30));
        bool requestTravelStop = _builderUnit.TryRequestTravelStop();
        
        Assert.True(requestTravelStop);
        Assert.Equal(_sol, _builderUnit.Location.System);
        Assert.Null(_builderUnit.Location.Planet);
    }
}