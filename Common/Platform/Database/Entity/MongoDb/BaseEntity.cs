using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Platform.Database.Entity.MongoDb;

public class BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
}