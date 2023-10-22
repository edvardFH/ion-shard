namespace IonShard.Contracts.DTO.Units;

public class UnitDTO
{
    public string Id { get; }
    public string Type { get; }
    public string System { get; }
    public string? Planet { get; }
    public string? DestinationSystem { get; }
    public string? DestionPlanet { get; }
    public DateTime? EstimatedTimeOfArrival { get; }

    public UnitDTO(string id, string type, string system, string? planet, string? destinationSystem, string? destionPlanet, DateTime? estimatedTimeOfArrival)
    {
        Id = id;
        Type = type;
        System = system;
        Planet = planet;
        DestinationSystem = destinationSystem;
        DestionPlanet = destionPlanet;
        EstimatedTimeOfArrival = estimatedTimeOfArrival;
    }
}
