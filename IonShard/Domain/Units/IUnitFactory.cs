using IonShard.Domain.Buildings;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Users;

namespace IonShard.Domain.Units;

public interface IUnitFactory
{
    public IUnit CreateUnitWithId
        (
            string id,
            IUser owner,
            StarSystem system,
            Planet? planet,
            string type,
            IBuildingFactory? buildingFactory,
            int healthPoints = -1,
            IReadOnlyDictionary<IResource, int>? loadedResources = null
        ); 
    
    public IUnit CreateNewUnit
        (
            IUser owner,
            StarSystem system,
            Planet? planet,
            string type,
            IBuildingFactory? buildingFactory
        );

    public bool DoesTypeExist(string type);
}
