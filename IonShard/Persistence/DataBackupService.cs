using IonShard.Persistence.Database;
using IonShard.Persistence.Repositories;

namespace IonShard.Persistence;

public class DataBackupService: IDataBackupService
{
    private readonly MapRepository _mapRepository;
    private readonly IShardDatabaseService _database;

    public DataBackupService(MapRepository mapRepository, IShardDatabaseService database)
    {
        _mapRepository = mapRepository;
        _database = database;
    }

    public void BackupData()
    {
        _database.UpdateMap(_mapRepository.Systems);
    }
}
