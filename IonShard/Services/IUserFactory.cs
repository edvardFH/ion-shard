using IonShard.Domain.Users;

namespace IonShard.Services;

public interface IUserFactory
{
    public IUser GetNewUser(string id, string pseudo);
}
