using Catalog.API.Common;
using Catalog.API.Common.Consts;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Entities;

[BsonCollection(DatabaseConst.CollectionName.Product)]
public class Product : BaseEntity
{
    [BsonElement("Name")]
    public string? Name { get;  set; }

    public string? Summary { get; set; }

    public string? Description { get; set; }

    public string? ImageFile { get; set; }

    public decimal Price { get; set; }

    public string? CategoryId { get; set; }
    public string? SubCategoryId { get; set; }
}
