using IonShard.Adapters.Mappers;
using IonShard.Domain.Map;
using IonShard.Persistence.Database;
using IonShard.Persistence.Repositories;
using Shard.Shared.Core;

namespace IonShard.Persistence.Loaders;

public class MapLoaderService
{
    public MapLoaderService
        (
            IShardDatabaseService database,
            MapGenerator mapGenerator,
            MapMapper mapMapper,
            MapRepository mapRepository
        )
    {
        var storedMap = database.GetMap();

        var universe = storedMap.Count > 0
            ? new Universe(storedMap)
            : mapMapper.SectorSpecificationToUniverse(mapGenerator.Generate());

        mapRepository.InitUniverse(universe);
    }
}
