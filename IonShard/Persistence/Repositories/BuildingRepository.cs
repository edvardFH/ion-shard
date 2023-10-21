using IonShard.Domain.Buildings;

namespace IonShard.Persistence.Repositories;

public class BuildingRepository
{
    private readonly IDictionary<string, Building> _buildings = new Dictionary<string, Building>();
    public IDictionary<string, Building> Buildings { get => _buildings; }

    public Building? this[string id] => _buildings.ContainsKey(id)
        ? _buildings[id]
        : null;
}
