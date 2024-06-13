using ApiClient.Discount.Models.Discount;
using ApiClient.Discount.Models.Discount.AmountModel;
using Discount.Domain.Repositories.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Discount.Domain.Repositories;

public interface IDiscountRepository
{
    Task<Entities.Discount?> GetDiscountAsync(string id);
    Task<List<Entities.Discount>?> GetListDiscountsByCatalogCodeAsync(DiscountEnum type, List<string> catalogCodes);
    Task<Entities.Discount?> GetDiscountByCatalogCodeAsync(DiscountEnum type, string catalogCode);
    Task<Entities.Discount> CreateDiscountAsync(Entities.Discount Discount);
    Task<Entities.Discount> UpdateDiscountAsync(Entities.Discount Discount);
    Task<bool> DeleteDiscountAsync(int id);
    Task<bool> AnyDateAsync(string catalogCode, DiscountEnum type, DateTime? fromDate, DateTime? toDate);
    //Task<Entities.Discount?> AmountDiscountAsync(AmountDiscountRequestBody requestBody);
}

public class DiscountRepository : IDiscountRepository
{
    private readonly IBaseRepository _baseRepository;

    public DiscountRepository(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
    }

    public async Task<bool> AnyDateAsync(string catalogCode, DiscountEnum type, DateTime? fromDate, DateTime? toDate)
    {
        //ToDo Will implement for null DateTime later
        if (fromDate is null || toDate is null)
        {
            return true;
        }

        const string query = "CatalogCode = @CatalogCode and Type = @Type and (ToDate >= @FromDate or FromDate <= @ToDate)";
        object param = new { CatalogCode = catalogCode, Type = (int)type, FromDate = fromDate.Value, ToDate = toDate.Value };
        var isOverlap = await _baseRepository.AnyAsync<Entities.Discount>(query, param);

        return isOverlap;
    }

    public async Task<Entities.Discount?> GetDiscountByCatalogCodeAsync(DiscountEnum type, string catalogCode)
    {
        const string query = "CatalogCode = @CatalogCode and Type = @Type";
        object param = new { CatalogCode = catalogCode, Type = (int)type };

        var entity = await _baseRepository.QueryFirstOrDefaultAsync<Entities.Discount>(query, param);

        return entity;
    }

    public async Task<Entities.Discount?> GetDiscountAsync(string id)
    {
        const string query = "Id = @Id";

        var entity = await _baseRepository.QueryFirstOrDefaultAsync<Entities.Discount>(query, new { Id = int.Parse(id) });

        return entity;
    }

    public async Task<List<Entities.Discount>?> GetListDiscountsByCatalogCodeAsync(DiscountEnum type, List<string> catalogCodes)
    {
        const string query = "CatalogCode IN (@CatalogCodes) and Type = @Type";
        object param = new { CatalogCodes = string.Join(",", catalogCodes), Type = (int)type };

        var entity = await _baseRepository.QueryAsync<Entities.Discount>(query, param);

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

    //public async Task<Entities.Discount> AmountDiscountAsync(List<CatalogItem> catalogItems)
    //{
    //    const string query = "";
    //    var param = new { RequestBody = requestBody };
    //    var entity = await _baseRepository.QueryAsync<Entities.Discount>(query, param);
    //}
}