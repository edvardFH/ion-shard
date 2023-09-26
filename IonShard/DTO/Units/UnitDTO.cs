using IonShard.DTO.Map;

namespace IonShard.DTO.Units;

public class UnitDTO
{
    public string Id { get; }
    public string Type => "scout";
    public StarSystemDTO System { get; }
    public PlanetDTO? Planet { get; }

    public UnitDTO(string id, StarSystemDTO system, PlanetDTO? planet)
    {
        Id = id;
        System = system;
        Planet = planet;
    }

}
