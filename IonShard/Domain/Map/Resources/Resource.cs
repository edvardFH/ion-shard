namespace IonShard.Domain.Map.Resources;

public record Resource: IResource
{
    public ResourceName Name { get; }
    public ResourceState State { get; }

    public Resource(ResourceName name)
    {
        Name = name;
        State = name switch
        {
            ResourceName.Oxygen => ResourceState.Gaseous,
            ResourceName.Water => ResourceState.Liquid,
            _ => ResourceState.Solid
        };
    }
}

