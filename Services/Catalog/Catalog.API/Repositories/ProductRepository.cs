using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProducts();
    Task<Product> GetProductById(string id);
    Task<IEnumerable<Product>> GetProductByName(string name);
    Task<IEnumerable<Product>> GetProductByCategory(string categoryName);

    Task CreateProduct(Product product);

    Task<bool> UpdateProduct(Product product);
    Task<bool> DeleteProduct(string id);
}

public class ProductRepository : IProductRepository
{
    private readonly ICatologContext _catologContext;

    public ProductRepository(ICatologContext catologContext)
    {
        _catologContext = catologContext ?? throw new ArgumentNullException(nameof(catologContext));
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _catologContext.Products.Find(x => true).ToListAsync();
    }

    public async Task<Product> GetProductById(string id)
    {
        return await _catologContext.Products.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    //Eq and ElementMatch of Mongo???
    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Name, name);

        return await _catologContext.Products.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Category, categoryName);

        return await _catologContext.Products.Find(filter).ToListAsync();
    }

    public async Task CreateProduct(Product product)
    {
        await _catologContext.Products.InsertOneAsync(product);
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var updateResult = await _catologContext.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Id, id);
        DeleteResult deleteResult = await _catologContext.Products.DeleteOneAsync(filter);

        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}
