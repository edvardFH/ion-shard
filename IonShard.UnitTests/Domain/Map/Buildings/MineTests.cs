
using IonShard.Configuration.Gamerules;
using IonShard.Domain.Buildings;
using IonShard.Domain.Buildings.Mine;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units.Builder;
using IonShard.Domain.Users;
using IonShard.UnitTests.Repository;
using Moq;
using Shard.Shared.Web.IntegrationTests.Clock;

namespace IonShard.UnitTests.Domain.Map.Buildings;

public class MineTests
{
    private readonly Planet _earth;
    private readonly StarSystem _sol;
    private readonly IBuilderUnit _builderUnit;
    private readonly FakeClock _clock;
    private readonly IUser _user;

    public MineTests()
    {
        var mockResourceFactory = new Mock<IResourceFactory>();
        var mockGameRulesService = new Mock<IGameRulesService>();
        var mockBuildingFactory = new Mock<IBuildingFactory>();
        
        _clock = new FakeClock();
        _earth = new Planet("earth", 12742, ResourcesTestProvider.GetResources());
        _sol = new StarSystem("sol", new [] { _earth });
        _user = new User("1", "john.doe", DateTime.Now, mockGameRulesService.Object, mockResourceFactory.Object);
        _builderUnit = new BuilderUnit("id1", mockBuildingFactory.Object, _user, _sol, _earth, null, 100, _clock);
    }

    [Fact]
    public void MineCollectsLiquidFromPlanet()
        => ExecuteTest(ResourceCategory.Liquid);
    
    [Fact]
    public void MineCollectsGasFromPlanet()
        => ExecuteTest(ResourceCategory.Gaseous);

    [Fact]
    public void MineCollectsSolidFromPlanet()
        => ExecuteTest(ResourceCategory.Solid);

    private async void ExecuteTest(ResourceCategory category)
    {
        // Creating the mine
        _ = new MineBuilding(_builderUnit, _sol, _earth, category, _clock, null, true);
        
        // Retrieving the rarest resource for the given category and its initial quantity
        var resourceToCheck = _earth
            .ResourcesQuantity.Where(pair => pair.Key.Category == category)
            .OrderByDescending(pair => pair.Key.Rarity)
            .Select(pair => pair.Key).FirstOrDefault()!;
        var initialQty = _earth
            .ResourcesQuantity.Where(pair => pair.Key == resourceToCheck).Select(pair => pair.Value).FirstOrDefault();
        
        // Waiting for the first period (mine building time + period)
        await _clock.Advance(TimeSpan.FromMinutes(6));
        
        // Assertions
        Assert.Equal(initialQty - 1, _earth
            .ResourcesQuantity.Where(pair => pair.Key == resourceToCheck).Select(pair => pair.Value).FirstOrDefault());
        Assert.Equal(1, _user
            .ResourcesQuantity.Where(pair => pair.Key == resourceToCheck).Select(pair => pair.Value).FirstOrDefault());
    }
}