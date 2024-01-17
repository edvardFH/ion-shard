namespace IonShard.Persistence.POCOs;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;

public class StarSystemPOCO
{
    [BsonId]
    [BsonIgnoreIfDefault]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId InternalId { get; init; }

    public string Name { get; init; }
    public IReadOnlyList<PlanetPOCO> Planets { get; init; }
}

