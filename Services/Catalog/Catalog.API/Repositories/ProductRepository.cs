using ApiClient.Catalog.Models;
using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public interface IProductRepository
{
    Task<List<ProductSummary>> GetProductsAsync(CancellationToken cancellationToken);
    Task<Product> GetProductByIdAsync(string id, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetProductByNameAsync(string name, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryName, CancellationToken cancellationToken);
    Task CreateProductAsync(Product product, CancellationToken cancellationToken);
    Task<bool> UpdateProductAsync(Product product, CancellationToken cancellationToken);
    Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken);
}

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _catologContext;

    public ProductRepository(ICatalogContext catologContext)
    {
        _catologContext = catologContext ?? throw new ArgumentNullException(nameof(catologContext));
    }

    public async Task<List<ProductSummary>> GetProductsAsync(CancellationToken cancellationToken)
    {
        var entities = await _catologContext.Products.Find(x => true).ToListAsync(cancellationToken);

        return entities.Select(x => x.ToSummary()).ToList();
    }

    public async Task<Product> GetProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _catologContext.Products.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    //Eq and ElementMatch of Mongo???
    public async Task<IEnumerable<Product>> GetProductByNameAsync(string name, CancellationToken cancellationToken)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Name, name);

        return await _catologContext.Products.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryName, CancellationToken cancellationToken)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Category, categoryName);

        return await _catologContext.Products.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task CreateProductAsync(Product product, CancellationToken cancellationToken)
    {
        var options = new InsertOneOptions()
        {
            BypassDocumentValidation = false
        };
        
        await _catologContext.Products.InsertOneAsync(product, options, cancellationToken);
    }

    public async Task<bool> UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        var options = new ReplaceOptions()
        {
            BypassDocumentValidation = false,
        };
        
        var updateResult = await _catologContext.Products.ReplaceOneAsync(g => g.Id == product.Id, product, options, cancellationToken);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Id, id);
        DeleteResult deleteResult = await _catologContext.Products.DeleteOneAsync(filter, cancellationToken);

        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}
