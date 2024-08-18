using Catalog.API.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Platform.Constants;

namespace Catalog.API.Entities;

[BsonCollection(DatabaseConst.ConnectionSetting.MongoDB.CollectionName.ProductHistory)]
public class ProductHistory : BaseEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string ProductId { get; set; } = null!;
    
    public int Balance { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public string CreatedBy { get; set; } = null!;
}