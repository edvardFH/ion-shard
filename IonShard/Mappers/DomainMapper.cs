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
        var unitSystem = unit.Location.System;
        var unitPlanet = unit.Location.Planet;

        return new UnitDTO(
            unit.Id,
            new StarSystemDTO(unitSystem.Name, unitSystem.Planets),
            unitPlanet is not null
                ? new PlanetDTO(unitPlanet.Name, unitPlanet.Size)
                : null);
    }
}
