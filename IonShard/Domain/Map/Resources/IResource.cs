namespace IonShard.Domain.Map.Resources;

public interface IResource
{
    public string Name { get; }
    public ResourceCategory Category { get; }
    public int Rarity { get; }
}
