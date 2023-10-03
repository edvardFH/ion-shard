using IonShard.Domain.Users;
using IonShard.DTO.Users;

namespace IonShard.Mappers;

public static class DomainMapper
{
    public static UserDTO ToDTO(this User user)
    {
        return new UserDTO(user.Id, user.Pseudo, user.DateOfCreation);
    }
}
