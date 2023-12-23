using IonShard.Configuration.Gamerules;
using IonShard.Configuration.Gamerules.Units;
using IonShard.Domain.Buildings;
using IonShard.Domain.Buildings.Starport;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units;
using IonShard.Domain.Units.Builder;
using IonShard.Domain.Users;
using IonShard.UnitTests.Repository;
using Moq;
using Shard.Shared.Web.IntegrationTests.Clock;

namespace IonShard.UnitTests.Domain.Map.Buildings;

public class StarportTests
{
    private readonly Planet _earth;
    private readonly StarSystem _sol;
    private readonly IBuilderUnit _builderUnit;
    private readonly FakeClock _clock;
    private readonly IUser _user;
    private readonly IStarportBuilding _starport;
    private readonly IResource _titanium;

    public StarportTests()
    {
        var mockResourceFactory = new Mock<IResourceFactory>();
        var mockGameRulesService = new Mock<IGameRulesService>();
        var mockBuildingFactory = new Mock<IBuildingFactory>();
        var mockUnitFactory = new Mock<IUnitFactory>();
        
        _titanium = new Resource("Titanium", ResourceCategory.Solid, 50);
        _clock = new FakeClock();
        _earth = new Planet("earth", 12742, ResourcesTestProvider.GetResources());
        _sol = new StarSystem("sol", new [] { _earth });
        _user = new User("1", "john.doe", DateTime.Now, mockGameRulesService.Object, mockResourceFactory.Object);

        SetupMocks(mockResourceFactory, mockGameRulesService, mockBuildingFactory, mockUnitFactory);
        
        _builderUnit = new BuilderUnit("id1", mockBuildingFactory.Object, _user, _sol, _earth, null, 100, _clock);
        _starport = new StarportBuilding(_builderUnit, _sol, _earth, _clock, mockUnitFactory.Object, mockBuildingFactory.Object, null, true);
    }

    private void SetupMocks(Mock<IResourceFactory> mockResourceFactory, Mock<IGameRulesService> mockGameRulesService, Mock<IBuildingFactory> mockBuildingFactory, Mock<IUnitFactory> mockUnitFactory)
    {
        // Arrange
        var resourceCost = new Dictionary<string, int>() { {"Titanium", 5} };
        var dictionary = new Dictionary<string, IUnitConfiguration>();
        dictionary.Add("builder", new UnitConfiguration(resourceCost, 60, 100));
        mockGameRulesService.SetupGet(service => service.Units).Returns(dictionary);

        mockResourceFactory
            .Setup(factory => factory.GetResource("Titanium"))
            .Returns(_titanium);

        mockUnitFactory
            .Setup(factory => 
                factory.CreateNewUnit(It.IsAny<IUser>(), 
                    It.IsAny<StarSystem>(), 
                    It.IsAny<Planet>(), 
                    It.IsAny<string>(), 
                    It.IsAny<IBuildingFactory>()))
            .Returns(new BuilderUnit("2",
                mockBuildingFactory.Object, 
                _user, 
                _sol, 
                _earth, 
                new Dictionary<IResource, int>() {{_titanium, 5}}, 
                100,
                _clock));
    }
    
    [Fact]
    public void AddUnitToQueue_UserWithNoResources()
    {
        Assert.Throws<InvalidOperationException>(() => _starport.AddToQueue("builder"));
    }

    [Fact]
    public void AddUnitToQueue_UserWithResources()
    {
        _user.AddResource(_titanium, 10);
        var newBuilderUnit = _starport.AddToQueue("builder");
        
        Assert.NotNull(newBuilderUnit);
        Assert.Equal(5, _user.ResourcesQuantity
            .Where(pair => pair.Key.Name == "Titanium").Select(pair => pair.Value).FirstOrDefault());
    }
}