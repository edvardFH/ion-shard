using IonShard.Domain.Units;
using IonShard.Domain.Users;
using IonShard.DTO.Map;
using IonShard.DTO.Units;
using IonShard.DTO.Users;

namespace IonShard.Mappers;

public static class DomainMapper
{
    public static UserDTO ToDTO(this User user)
    {
        return new UserDTO(user.Id, user.Pseudo, user.DateOfCreation);
    }

    public static UnitDTO ToDTO(this Unit unit)
    {
        var unitLocation = unit.Location;

        return new UnitDTO(
            unit.Id,
            unitLocation.System.Name,
            unitLocation.Planet?.Name);
    }

    public static UnitLocationDTO ToLocationDTO(this Unit unit)
    {
        var unitLocation = unit.Location;

        return new UnitLocationDTO(
            unitLocation.System.Name,
            unitLocation.Planet?.Name,
            unit.Location.Planet?.ResourcesQuantity
                .ToDictionary(resource => resource.Key.ToString().ToLower(), resource => resource.Value));
    }
}
