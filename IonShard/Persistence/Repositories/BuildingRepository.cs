using IonShard.Domain.Buildings;

namespace IonShard.Persistence.Repositories;

public class BuildingRepository
{
    private readonly IDictionary<string, IBuilding> _buildings = new Dictionary<string, IBuilding>();
    public IDictionary<string, IBuilding> Buildings { get => _buildings; }

    public IBuilding? this[string id] => _buildings.ContainsKey(id)
        ? _buildings[id]
        : null;
}
