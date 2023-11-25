namespace IonShard.Domain.Map.Resources;

public interface IResource
{
    public ResourceName Name { get; }
    public ResourceCategory Category { get; }
}
