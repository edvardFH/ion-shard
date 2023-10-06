namespace IonShard.Contracts.DTO.Units;

public class UnitDTO
{
    public string Id { get; }
    public string Type => "scout";
    public string System { get; }
    public string? Planet { get; }

    public UnitDTO(string id, string system, string? planet)
    {
        Id = id;
        System = system;
        Planet = planet;
    }

}
