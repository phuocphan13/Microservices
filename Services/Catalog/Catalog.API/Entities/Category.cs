using Catalog.API.Common;
using Catalog.API.Common.Consts;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Entities;

[BsonCollection(DatabaseConst.CollectionName.Category)]
public class Category : BaseEntity
{
    [BsonElement("Name")] 
    public string? Name { get; set; }
    
    public string? CategoryCode { get; set; }
    
    public string? Description { get; set; }
}