using Discount.Domain.Entities;
using Discount.Domain.Repositories.Common;

namespace Discount.Domain.Repositories;

public interface ICouponRepository
{
    Task<Coupon?> GetCouponAsync(string value, CatalogType type);
    Task<Coupon> CreateCouponAsync(Coupon coupon);
    Task<Coupon> UpdateCouponAsync(Coupon coupon);
    Task<bool> DeleteCouponAsync(int id);
}

public class CouponRepository : ICouponRepository
{
    private readonly IBaseRepository _baseRepository;

    public CouponRepository(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
    }

    public async Task<Coupon?> GetCouponAsync(string value, CatalogType type)
    {
        const string query = "Code = @Code and @Type = Type";

        var coupon = await _baseRepository.QueryFirstOrDefaultAsync<Coupon>(query, new { Code = value, Type = (int)type });

        return coupon;
    }

    public async Task<Coupon> CreateCouponAsync(Coupon coupon)
    {
        var entity = await _baseRepository.CreateEntityAsync(coupon);
        return entity;
    }

    public async Task<Coupon> UpdateCouponAsync(Coupon coupon)
    {
        var entity = await _baseRepository.UpdateEntityAsync(coupon);
        return entity;
    }

    public async Task<bool> DeleteCouponAsync(int id)
    {
        var result = await _baseRepository.DeleteEntityAsync<Coupon>(id);

        return result;
    }
}