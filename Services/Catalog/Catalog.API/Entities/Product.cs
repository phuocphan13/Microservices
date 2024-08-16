using Catalog.API.Common;
using MongoDB.Bson.Serialization.Attributes;
using Platform.Constants;

namespace Catalog.API.Entities;

[BsonCollection(DatabaseConst.ConnectionSetting.MongoDB.CollectionName.Product)]
public class Product : BaseEntity
{
    [BsonElement("Name")]
    public string? Name { get;  set; }
    
    public string? ProductCode { get; set; }

    public string? Summary { get; set; }

    public string? Description { get; set; }

    public string? ImageFile { get; set; }

    public decimal Price { get; set; }
    
    public int Balance { get; set; }

    [BsonElement("CategoryId")]
    public string? CategoryId { get; set; }

    [BsonElement("SubCategoryId")]
    public string? SubCategoryId { get; set; }
}
