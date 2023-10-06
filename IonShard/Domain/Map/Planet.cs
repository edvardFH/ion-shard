namespace IonShard.Domain.Map
{
    public class Planet
    {
        public string Name { get; }
        public int Size { get; }
        public IReadOnlyDictionary<Resource, int> ResourcesQuantity { get; }

        public Planet(string name, int size, IReadOnlyDictionary<Resource, int> resourcesQuantity)
        {
            Name = name;
            Size = size;
            ResourcesQuantity = resourcesQuantity;
        }
    }
}
