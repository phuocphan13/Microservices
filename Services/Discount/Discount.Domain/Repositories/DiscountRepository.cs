using Discount.Domain.Entities;
using Discount.Domain.Repositories.Common;

namespace Discount.Domain.Repositories;

public interface IDiscountRepository
{
    Task<Coupon?> GetDiscountAsync(string value, CatalogType type);
    Task<Coupon> CreateDiscountAsync(Coupon coupon);
    Task<Coupon> UpdateDiscountAsync(Coupon coupon);
    Task<bool> DeleteDiscountAsync(int id);
}

public class DiscountRepository : IDiscountRepository
{
    private readonly IBaseRepository _baseRepository;

    public DiscountRepository(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
    }

    public async Task<Coupon?> GetDiscountAsync(string value, CatalogType type)
    {
        const string query = "Code = @Code and @Type = Type";

        var coupon = await _baseRepository.QueryFirstOrDefaultAsync<Coupon>(query, new { Code = value, Type = (int)type });

        return coupon;
    }

    public async Task<Coupon> CreateDiscountAsync(Coupon coupon)
    {
        var entity = await _baseRepository.CreateEntityAsync(coupon);
        return entity;
    }

    public async Task<Coupon> UpdateDiscountAsync(Coupon coupon)
    {
        var entity = await _baseRepository.UpdateEntityAsync(coupon);
        return entity;
    }

    public async Task<bool> DeleteDiscountAsync(int id)
    {
        var result = await _baseRepository.DeleteEntityAsync<Coupon>(id);

        return result;
    }

    // public async Task<bool> UpdateDiscount(Coupon coupon)
    // {
    //     using var connection = InitializaCollection();
    //     var affected = await connection.ExecuteAsync("UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
    //         new { coupon.Id, coupon.ProductName, coupon.Description, coupon.Amount });
    //
    //     return affected > 0;
    // }
    //
    // public async Task<bool> DeleteDiscount(string productName)
    // {
    //     using var connection = InitializaCollection();
    //     var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });
    //
    //     return affected > 0;
    // }
}