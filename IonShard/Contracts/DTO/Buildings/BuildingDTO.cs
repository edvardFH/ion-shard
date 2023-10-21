namespace IonShard.Contracts.DTO.Buildings;

public class BuildingDTO
{
    public string Id { get; }
    public string Type { get; }
    public string System { get; }
    public string? Planet { get; }

    public BuildingDTO(string id, string type, string system, string? planet)
    {
        Id = id;
        Type = type;
        System = system;
        Planet = planet;
    }
}
