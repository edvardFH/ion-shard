using IonShard.Domain.Users;

namespace IonShard.Services
{
    public class UsersRepository
    {
        private readonly IDictionary<string, User> _users;

        public UsersRepository(IDictionary<string, User> users)
        {
            _users = users;
        }
    }
}
