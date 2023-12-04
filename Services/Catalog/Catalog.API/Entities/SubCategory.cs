using Catalog.API.Common;
using Catalog.API.Common.Consts;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Entities;

[BsonCollection(DatabaseConst.CollectionName.SubCategory)]
public class SubCategory : BaseEntity
{
    [BsonElement("Name")] 
    public string? Name { get; set; }

    public string? Description { get; set; }
    
    public string? CategoryId { get; set; }
}