using MongoDB.Bson.Serialization.Attributes;
using Platform.Constants;
using Platform.Database.Entity.MongoDb;
using Platform.Database.MongoDb;

namespace Catalog.API.Entities;

[BsonCollection(DatabaseConst.ConnectionSetting.MongoDB.CollectionName.SubCategory)]
public class SubCategory : BaseEntity
{
    [BsonElement("Name")] 
    public string? Name { get; set; }

    public string? SubCategoryCode { get; set; }
    
    public string? Description { get; set; }
    
    public string? CategoryId { get; set; }
}