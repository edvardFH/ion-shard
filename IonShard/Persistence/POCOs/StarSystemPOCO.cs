namespace IonShard.Persistence.POCOs;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

public class StarSystemPOCO
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId InternalId { get; set; }

    public string Name { get; set; }
    public List<PlanetPOCO> Planets { get; set; }
}

