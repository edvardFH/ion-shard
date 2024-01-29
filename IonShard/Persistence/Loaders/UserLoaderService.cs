using IonShard.Domain.Users;
using IonShard.Persistence.Database;
using IonShard.Persistence.Repositories;

namespace IonShard.Persistence.Loaders;

public class UserLoaderService
{
    public UserLoaderService
        (
            IShardDatabaseService database,
            UserRepository userRepository
        )
    {
        database.GetUsers().ToList().ForEach(user => userRepository.Users.Add(user.Id, user));
    }
}
