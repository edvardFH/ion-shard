using IonShard.Persistence.Database;
using IonShard.Persistence.Repositories;
using Shard.Shared.Core;

namespace IonShard.Persistence;

public class DataBackupService: IDataBackupService
{
    private const int BackupPeriod = 60;
    private const int DueTime = 50;

    private readonly MapRepository _mapRepository;
    private readonly UserRepository _userRepository;
    private readonly IShardDatabaseService _database;

    private readonly ITimer _timer;


    public DataBackupService
        (
            MapRepository mapRepository,
            UserRepository userRepository,
            IShardDatabaseService database,
            IClock clock
        )
    {
        _mapRepository = mapRepository;
        _userRepository = userRepository;
        _database = database;

        var period = TimeSpan.FromSeconds(BackupPeriod);

        _timer = clock.CreateTimer
            (
                (state) => BackupData(),
                null,
                TimeSpan.FromSeconds(DueTime),
                TimeSpan.FromSeconds(BackupPeriod)
            );
    }

    public void BackupData()
    {
        _database.UpdateMapAsync(_mapRepository.Systems);
        _database.UpdateUsersAsync(_userRepository.Users
            .ToList()
            .Select(keyValuePair => keyValuePair.Value));

        Console.WriteLine("Backup done.");
    }
}
