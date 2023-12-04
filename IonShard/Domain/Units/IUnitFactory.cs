using IonShard.Domain.Map;
using IonShard.Domain.Users;

namespace IonShard.Domain.Units;

public interface IUnitFactory
{
    public IUnit GetNewUnit
        (
            IUser owner,
            StarSystem system,
            Planet? planet,
            string type
        );

    public bool DoesTypeExist(string type);
}
