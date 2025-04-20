using MongoDB.Bson.Serialization.Attributes;
using Platform.Constants;
using Platform.Database.Entity.MongoDb;
using Platform.Database.MongoDb;

namespace Catalog.API.Entities;

[BsonCollection(DatabaseConst.ConnectionSetting.MongoDB.CollectionName.Category)]
public class Category : BaseEntity
{
    [BsonElement("Name")] 
    public string? Name { get; set; }
    
    public string? CategoryCode { get; set; }
    
    public string? Description { get; set; }
}