using IonShard.Domain.Map.Resources;
using IonShard.Domain.Users;

namespace IonShard.Services;

public interface IUserFactory
{
    public IUser CreateNewUser(string id, string pseudo);

    public IUser CreateUser
        (
            string id,
            string pseudo,
            DateTime dateOfCreation,
            IReadOnlyDictionary<IResource, int> resourcesQuantity
        );
}
