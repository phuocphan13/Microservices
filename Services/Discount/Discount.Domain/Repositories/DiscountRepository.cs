using Discount.Domain.Entities;
using Discount.Domain.Repositories.Common;

namespace Discount.Domain.Repositories;

public interface IDiscountRepository
{
    Task<Entities.Discount?> GetDiscountAsync(string id);
    Task<Entities.Discount> CreateDiscountAsync(Entities.Discount Discount);
    Task<Entities.Discount> UpdateDiscountAsync(Entities.Discount Discount);
    Task<bool> DeleteDiscountAsync(int id);
    Task<bool> AnyDateAsync(DateTime? fromDate, DateTime? toDate);
}

public class DiscountRepository : IDiscountRepository
{
    private readonly IBaseRepository _baseRepository;

    public DiscountRepository(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
    }

    public async Task<bool> AnyDateAsync(DateTime? fromDate, DateTime? toDate)
    {
        //ToDo Will implement for null DateTime later
        if (fromDate is null || toDate is null)
        {
            return true;
        }

        const string query = $"@fromDate <= ToDate or @toDate >= FromDate";
        object param = new { fromDate, toDate };
        var isOverlap = await _baseRepository.AnyAsync<Entities.Discount>(query, param);

        return isOverlap;
    }
    
    public async Task<Entities.Discount?> GetDiscountAsync(string id) 
    {
        const string query = "Id = @Id";

        var entity = await _baseRepository.QueryFirstOrDefaultAsync<Entities.Discount>(query, new { Id = id });

        return entity;
    }

    public async Task<Entities.Discount> CreateDiscountAsync(Entities.Discount Discount) 
    {
        var entity = await _baseRepository.CreateEntityAsync(Discount);
        return entity;
    }

    public async Task<Entities.Discount> UpdateDiscountAsync(Entities.Discount Discount) 
    {
        var entity = await _baseRepository.UpdateEntityAsync(Discount);
        return entity;
    }

    public async Task<bool> DeleteDiscountAsync(int id) 
    {
        var result = await _baseRepository.DeleteEntityAsync<Entities.Discount>(id);

        return result;
    }
}