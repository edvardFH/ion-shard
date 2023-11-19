using IonShard.Domain.Users;

namespace IonShard.Persistence.Repositories;

public class UserRepository
{
    private readonly IDictionary<string, IUser> _users = new Dictionary<string, IUser>();
    public IDictionary<string, IUser> Users { get => _users; }

    public IUser? this[string id] => _users.ContainsKey(id)
        ? _users[id]
        : null;
}
