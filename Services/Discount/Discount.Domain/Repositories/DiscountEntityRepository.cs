using Discount.Domain.Entities;
using Discount.Domain.Repositories.Common;

namespace Discount.Domain.Repositories;

public interface IDiscountEntityRepository
{
    Task<T?> GetDiscountAsync<T>(string id) where T : DiscountEntity, new();
    Task<T> CreateDiscountAsync<T>(T Discount) where T : DiscountEntity, new();
    Task<T> UpdateDiscountAsync<T>(T Discount) where T : DiscountEntity, new();
    Task<bool> DeleteDiscountAsync<T>(int id) where T : DiscountEntity, new();
}

public class DiscountEntityRepository : IDiscountEntityRepository
{
    private readonly IBaseRepository _baseRepository;

    public DiscountEntityRepository(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
    }

    public async Task<T?> GetDiscountAsync<T>(string id) 
        where T : DiscountEntity, new()
    {
        const string query = "Id = @Id";

        var entity = await _baseRepository.QueryFirstOrDefaultAsync<T>(query, new { Id = id });

        return entity;
    }

    public async Task<T> CreateDiscountAsync<T>(T Discount) 
        where T : DiscountEntity, new()
    {
        var entity = await _baseRepository.CreateEntityAsync<T>(Discount);
        return entity;
    }

    public async Task<T> UpdateDiscountAsync<T>(T Discount) 
        where T : DiscountEntity, new()
    {
        var entity = await _baseRepository.UpdateEntityAsync<T>(Discount);
        return entity;
    }

    public async Task<bool> DeleteDiscountAsync<T>(int id) 
        where T : DiscountEntity, new()
    {
        var result = await _baseRepository.DeleteEntityAsync<T>(id);

        return result;
    }
}