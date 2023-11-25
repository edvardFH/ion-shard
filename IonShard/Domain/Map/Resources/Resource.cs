namespace IonShard.Domain.Map.Resources;

public record Resource: IResource
{
    public ResourceName Name { get; }
    public ResourceCategory Category { get; }

    public Resource(ResourceName name)
    {
        Name = name;
        Category = name switch
        {
            ResourceName.Oxygen => ResourceCategory.Gaseous,
            ResourceName.Water => ResourceCategory.Liquid,
            _ => ResourceCategory.Solid
        };
    }
}

