using ApiClient.Discount.Models.Discount;
using Refit;

namespace ApiClient.Refits.Discount;

public interface IDiscountApiClient
{
    [Get("/api/v1/discount/GetDiscountByCatalogCode")]
    Task<DiscountDetail> GetDiscountByCatalogCode([Query] int type, [Query] string catalogCode); 
    
    [Get("/api/v1/discount/GetListDiscountsByCatalogCodeAsync")]
    Task<List<DiscountDetail>> GetDiscountByCatalogCode([Query] int type, [Query] List<string> catalogCodes);
}