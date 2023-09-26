namespace IonShard.Domain.Map
{
    public class Planet
    {
        public string Name { get; }
        public int Size { get; }
        public IReadOnlyDictionary<Resource, int> ResourceQuantity { get; }

        public Planet(string name, int size, IReadOnlyDictionary<Resource, int> resourceQuantity)
        {
            Name = name;
            Size = size;
            ResourceQuantity = resourceQuantity;
        }
    }
}
