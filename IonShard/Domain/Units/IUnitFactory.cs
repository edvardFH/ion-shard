using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Users;

namespace IonShard.Domain.Units;

public interface IUnitFactory
{
    public IUnit CreateUnit
        (
            IUser owner,
            StarSystem system,
            Planet? planet,
            string type,
            IBuildingFactory? buildingFactory
        );

    public bool DoesTypeExist(string type);
}
