using IonShard.Domain.Buildings.Starport;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Users;
using Shard.Shared.Core;

namespace IonShard.Domain.Units.Cargo;

public class CargoUnit : Unit, ICargoUnit
{
    private readonly IDictionary<IResource, int> _loadedResources;
    public IReadOnlyDictionary<IResource, int> LoadedResources
        => _loadedResources.AsReadOnly();


    public CargoUnit
        (
            string id,
            IUser owner,
            StarSystem system,
            Planet? planet,
            string type,
            IReadOnlyDictionary<IResource, int> resourceCost,
            IReadOnlyDictionary<IResource, int> loadedResources,
            IClock clock
        )
        : base(id, owner, system, planet, type, resourceCost, clock)
    {
        _loadedResources = new Dictionary<IResource, int>(loadedResources);
    }


    public int Load(IResource resource, int quantity)
    {
        if (!LocationContainsStarport())
            throw new InvalidOperationException("Cannot load cargo, its location does not contain a starport.");

        var ownerResourceQuantity = Owner.ResourcesQuantity.ContainsKey(resource)
            ? Owner.ResourcesQuantity[resource]
            : 0;

        if (ownerResourceQuantity < quantity)
            throw new InvalidOperationException(
                $"Cannot load {quantity} {resource.Name} in cargo, user only has {ownerResourceQuantity} of it.");

        Owner.UseResource(resource, quantity);

        if (_loadedResources.ContainsKey(resource))
           _loadedResources[resource] += quantity;
        else
            _loadedResources.Add(resource, quantity);

        return _loadedResources[resource];
    }

    public int Unload(IResource resource, int quantity)
    {
        if (!LocationContainsStarport())
            throw new InvalidOperationException("Cannot unload cargo, its location does not contain a starport.");

        var cargoResourceQuantity = LoadedResources.ContainsKey(resource)
            ? LoadedResources[resource]
            : 0;

        if (cargoResourceQuantity < quantity)
            throw new InvalidOperationException(
                $"Cannot unload {quantity} {resource.Name} from cargo, it only contains {cargoResourceQuantity} of it.");

        if(_loadedResources.ContainsKey(resource))
            _loadedResources[resource] -= quantity;
        else
            _loadedResources.Add(resource, 0);

        Owner.AddResource(resource, quantity);

        return _loadedResources[resource];
    }

    private bool LocationContainsStarport()
    {
        if (Location.Planet is null)
            return false;

        foreach(var building in Location.Planet.Buildings)
        {
            if(building is IStarportBuilding)
                return true;
        }

        return false;
    }
}
