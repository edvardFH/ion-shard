using IonShard.Domain.Users;

namespace IonShard.Services
{
    public class UserRepository
    {
        private readonly IDictionary<string, User> _users = new Dictionary<string, User>();
        public IDictionary<string, User> Users { get => _users; }

        public User? this[string id] => _users.ContainsKey(id)
            ? _users[id]
            : null;
    }
}
