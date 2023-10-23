namespace IonShard.Contracts.DTO.Units;

public class UnitDTO
{
    public string Id { get; }
    public string Type { get; }
    public string System { get; }
    public string? Planet { get; }
    public string? DestinationSystem { get; }
    public string? DestinationPlanet { get; }
    public string? EstimatedTimeOfArrival { get; }

    public UnitDTO(string id, string type, string system, string? planet, string? destinationSystem, string? destinationPlanet, string? estimatedTimeOfArrival)
    {
        Id = id;
        Type = type;
        System = system;
        Planet = planet;
        DestinationSystem = destinationSystem;
        DestinationPlanet = destinationPlanet;
        EstimatedTimeOfArrival = estimatedTimeOfArrival;
    }
}
