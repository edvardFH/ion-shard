using IonShard.Domain.Map;
using IonShard.Domain.Units;
using IonShard.Domain.Units.Builder;
using IonShard.Domain.Users;
using IonShard.UnitTests.Repository;
using Shard.Shared.Core;
using Shard.Shared.Web.IntegrationTests.Clock;

namespace IonShard.UnitTests.Domain.Units;

public class BuilderUnitTests
{
    private readonly LocalTestRepository _repository;
    private readonly StarSystem _sol;
    private readonly Planet _earth;
    private readonly FakeClock _clock;
    private readonly IUnit _builderUnit;
    private readonly IUnitFactory _unitFactory;

    public BuilderUnitTests()
    {
        _repository = LocalTestRepository.GetInstance();
        _sol = _repository["sol"]!;
        _earth = _sol["earth"]!;
        _clock = new FakeClock();

        IUser userJohn = _repository.UserRepository.Users["1"];
        _builderUnit = new BuilderUnit(null, userJohn, _sol, _earth, null, _clock);
    }

    [Fact]
    public async Task MoveUnit()
    {
        StarSystem alphaCentauri = _repository["alpha-centauri"]!;
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
        StarSystem alphaCentauri = _repository["alpha-centauri"]!;
        _builderUnit.StartTravel(_clock, alphaCentauri, null);
        
        await _clock.Advance(TimeSpan.FromSeconds(30));
        bool requestTravelStop = _builderUnit.TryRequestTravelStop();
        
        Assert.True(requestTravelStop);
        Assert.Equal(_sol, _builderUnit.Location.System);
        Assert.Null(_builderUnit.Location.Planet);
    }
}