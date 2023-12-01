using IonShard.Domain.Map;
using IonShard.Domain.Units;
using IonShard.Domain.Users;

namespace IonShard.Services;

public class UnitFactory
{
    private IConfiguration _configuration;
    public UnitFactory(IConfiguration configuration)
    {
        _configuration = configuration.GetSection("Units");
    }

    public IUnit GetNewUnit(IUser owner, StarSystem system, Planet? planet, string type)
    {
        return null;
    }
}
