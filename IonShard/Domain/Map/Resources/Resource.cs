namespace IonShard.Domain.Map.Resources;

public record Resource(string Name, ResourceCategory Category, int Rarity) : IResource;
