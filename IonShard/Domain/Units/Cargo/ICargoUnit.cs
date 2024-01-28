using IonShard.Domain.Map.Resources;

namespace IonShard.Domain.Units.Cargo;

public interface ICargoUnit : IUnit
{
    public IReadOnlyDictionary<IResource, int> LoadedResources { get; }

    public int Load(IResource resource, int quantity);
    public int Unload(IResource resource, int quantity);
}
