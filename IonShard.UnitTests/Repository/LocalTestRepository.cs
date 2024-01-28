using IonShard.Domain.Map;
using IonShard.Persistence.Repositories;
using IonShard.Domain.Users;

namespace IonShard.UnitTests.Repository;

public class LocalTestRepository
{
    private static LocalTestRepository _instance;
    private readonly Universe _universe;
    public Universe Universe { get => _universe; }
    private readonly UserRepository _userRepository;
    public UserRepository UserRepository { get => _userRepository; }

    private LocalTestRepository(List<StarSystem> systems)
    {
        _universe = new Universe(systems);
        _userRepository = new UserRepository();
        _userRepository.Users.Add("1", new User("1", "johndoe", DateTime.Now, null));
    }

    public StarSystem? this[string systemName] => _universe[systemName];

    public static LocalTestRepository GetInstance()
    {
        if (_instance is null)
        {
            _instance = new LocalTestRepository(StarSystemTestProvider.ProvideSystems());
        }

        return _instance;
    }
}