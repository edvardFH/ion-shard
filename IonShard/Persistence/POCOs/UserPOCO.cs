using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace IonShard.Persistence.POCOs;

public class UserPOCO
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; }

    public string Pseudo { get; init; }
    public DateTime DateOfCreation { get; init; }
    public IReadOnlyList<UnitPOCO> Units { get; init; }
    public IReadOnlyList<BuildingPOCO> Buildings { get; init; }
    public IReadOnlyDictionary<string, int> ResourcesQuantity { get; init; }
}