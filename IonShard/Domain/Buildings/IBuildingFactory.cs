using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units.Builder;

namespace IonShard.Domain.Buildings;

public interface IBuildingFactory
{
    public IBuilding CreateBuilding
        (
            string type,
            IBuilderUnit builder,
            StarSystem starSystem,
            Planet planet,
            bool isBuilt = false,
            DateTime? estimatedBuildTime = null,
            ResourceCategory? resourceCategory = null
        );

    public bool DoesTypeExist(string type);
}
