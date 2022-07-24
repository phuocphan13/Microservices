using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;

public interface ICatologContext
{
    IMongoCollection<Product> Products { get; }
}
