using IonShard.Domain.Users;

namespace IonShard.Services;

public interface IUserFactory
{
    public IUser CreateUser(string id, string pseudo);
}
