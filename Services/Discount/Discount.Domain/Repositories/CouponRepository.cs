using Discount.Domain.Entities;
using Discount.Domain.Repositories.Common;

namespace Discount.Domain.Repositories;

public interface ICouponRepository
{
    Task<Coupon?> GetDiscountAsync(string value, CatalogType type);
    Task<Coupon> CreateDiscountAsync(Coupon coupon);
    Task<Coupon> UpdateDiscountAsync(Coupon coupon);
    Task<bool> DeleteDiscountAsync(int id);
}

public class CouponRepository : ICouponRepository
{
    private readonly IBaseRepository _baseRepository;

    public CouponRepository(IBaseRepository baseRepository)
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
}