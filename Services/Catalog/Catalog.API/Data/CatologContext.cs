using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;

public interface ICatologContext
{
    IMongoCollection<Product> Products { get; }
}

public class CatologContext : ICatologContext
{
    public IMongoCollection<Product> Products { get; }

    public CatologContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

        Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
        CatologContextSeed.SeedData(Products);
    }


    
}
