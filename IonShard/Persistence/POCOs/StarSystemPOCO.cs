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
    public ObjectId InternalId { get; set; }

    public string Name { get; set; }
    public List<PlanetPOCO> Planets { get; set; }
}

