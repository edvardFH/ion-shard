using IonShard.Adapters.Mappers;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Persistence.Database;
using Shard.Shared.Core;

namespace IonShard.Persistence;

public class MapCreationService: IMapCreationService
{
    public Universe Map { get; }

    public MapCreationService(IShardDatabaseService database, MapGenerator mapGenerator, MapMapper mapMapper)
    {
        var storedMap = database.GetMap();

        if(storedMap.Count > 0)
            Map = new Universe(storedMap);
        else
            Map = mapMapper.SectorSpecificationToUniverse(mapGenerator.Generate());

        database.UpdateMap(Map.Systems);
    }
}
