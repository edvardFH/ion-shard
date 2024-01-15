using IonShard.Domain.Map;

namespace IonShard.Persistence;

public interface IMapCreationService
{
    public Universe Map { get; }
}
