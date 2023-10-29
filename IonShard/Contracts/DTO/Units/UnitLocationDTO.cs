namespace IonShard.Contracts.DTO.Units;

public class UnitLocationDTO
{
    public string System { get; }
    public string? Planet { get; }
    public IReadOnlyDictionary<string, int>? ResourcesQuantity { get; }

    public UnitLocationDTO(string system, string? planet, IReadOnlyDictionary<string, int>? resourcesQuantity)
    {
        System = system;
        Planet = planet;
        ResourcesQuantity = resourcesQuantity;
    }
}
