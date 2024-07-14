using ApiClient.Discount.Models.Discount.AmountModel;
using Discount.Domain.Models.EntityHelpers;
using Discount.Domain.Repositories.Common;
using System.Reflection;

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
    Task<List<Entities.Discount>?> AmountDiscountAsync(List<AmountDiscountRepositoryModel> requestBody);
    Task<List<Entities.Discount>?> GetAmountDiscountAsync(List<string> catalogItems);
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

    public async Task<List<Entities.Discount>?> GetListDiscountsByCatalogCodeAsync(DiscountEnum type, List<string> catalogCodes)// [111,222,333]
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

    public async Task<List<Entities.Discount>?> AmountDiscountAsync(List<AmountDiscountRepositoryModel> catalogItems) // truyen vo TYPE - LISTCATALOGCODE
    {
        List<string> prefixQueries = new List<string>();
        var param = new AmountDiscountParams();

        foreach (var item in catalogItems.OrderBy(x => x.Type))
        {
            string prefix = "";
            if (item.Type == "2")
            {
                prefix = "Cate";
                param.Type_Cate = 2;
                param.CatalogCode_Cate = string.Join(",", item.CatalogCodes);
            }
            else if (item.Type == "3")
            {
                prefix = "SubCate";
                param.Type_SubCate = 3;
                param.CatalogCode_SubCate = string.Join(",", item.CatalogCodes);
            }
            else if (item.Type == "4")
            {
                prefix = "Product";
                param.Type_Product = 4;
                param.CatalogCode_Product = string.Join(",", item.CatalogCodes);
            }

            string typePrefix = $"Type_{prefix}";
            string catalogCodePrefix = $"CatalogCode_{prefix}";

            prefixQueries.Add($"(Type = @{typePrefix} and CatalogCode in (@{catalogCodePrefix}))");
        }


        string query = string.Join(" or ", prefixQueries);
        var entity = await _baseRepository.QueryAsync<Entities.Discount>(query, param);
        return entity;
    }

    public async Task<List<Entities.Discount>?> GetAmountDiscountAsync(List<string> catalogItems)
    {
        const string query = "CatalogCode = ANY (@CatalogCodes)";
        object param = new { CatalogCodes =  catalogItems.ToArray()};
        var entity = await _baseRepository.QueryAsync<Entities.Discount>(query, param);

        return entity;
    }
}