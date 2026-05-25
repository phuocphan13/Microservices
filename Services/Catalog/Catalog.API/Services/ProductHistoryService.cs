using ApiClient.Catalog.ProductHistory.Models;
using Catalog.API.Entities;
using Catalog.API.Services.Caches;
using Platform.Common.Session;
using Platform.Database.MongoDb;

namespace Catalog.API.Services;

public interface IProductHistoryService
{
    Task<bool> AddProductBalanceAsync(AddProductBalanceRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<bool> AddHistoriesAsync(List<AddProductBalanceRequestBody> requestBodies, CancellationToken cancellationToken = default);
}

public class ProductHistoryService : IProductHistoryService
{
    private readonly IRepository<ProductHistory> _productHistoryRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly ISessionState _sessionState;

    public ProductHistoryService(IRepository<ProductHistory> productHistoryRepository, IRepository<Product> productRepository, 
        ISessionState sessionState)
    {
        _productHistoryRepository = productHistoryRepository;
        _productRepository = productRepository;
        _sessionState = sessionState;
    }

    public async Task<bool> AddHistoriesAsync(List<AddProductBalanceRequestBody> requestBodies, CancellationToken cancellationToken)
    {
        foreach (var body in requestBodies)
        {
            ProductHistory productHistory = new()
            {
                ProductId = body.Id,
                Balance = body.Balance,
                CreatedDate = DateTime.UtcNow
                // CreatedBy = _sessionState.GetUserId()
            };
            
            await _productHistoryRepository.CreateEntityAsync(productHistory, cancellationToken);
        }

        return true;
    }

    public async Task<bool> AddProductBalanceAsync(AddProductBalanceRequestBody requestBody, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetEntityFirstOrDefaultAsync(x => x.Id == requestBody.Id, cancellationToken);
        
        product.Balance += requestBody.Balance;

        var updateResult = await _productRepository.UpdateEntityAsync(product, cancellationToken);
        
        if (!updateResult)
        {
            return false;
        }

        ProductHistory productHistory = new()
        {
            ProductId = product.Id,
            Balance = requestBody.Balance,
            CreatedDate = DateTime.UtcNow
            // CreatedBy = _sessionState.GetUserId()
        };

        await _productHistoryRepository.CreateEntityAsync(productHistory, cancellationToken);

        return true;
    }
}