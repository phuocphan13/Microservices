using Discount.Domain.Repositories.Common;

namespace Discount.Domain.Repositories;

public interface ICouponRepository
{
    Task<Entities.Coupon?> GetCouponAsync(string value, CatalogType type);
    Task<Entities.Coupon> CreateCouponAsync(Entities.Coupon coupon);
    Task<Entities.Coupon> UpdateCouponAsync(Entities.Coupon coupon);
    Task<bool> DeleteCouponAsync(int id);
}

public class CouponRepository : ICouponRepository
{
    private readonly IBaseRepository _baseRepository;

    public CouponRepository(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
    }

    public async Task<Entities.Coupon?> GetCouponAsync(string value, CatalogType type)
    {
        const string query = "Code = @Code and @Type = Type";

        var coupon = await _baseRepository.QueryFirstOrDefaultAsync<Entities.Coupon>(query, new { Code = value, Type = (int)type });

        return coupon;
    }

    public async Task<Entities.Coupon> CreateCouponAsync(Entities.Coupon coupon)
    {
        var entity = await _baseRepository.CreateEntityAsync(coupon);
        return entity;
    }

    public async Task<Entities.Coupon> UpdateCouponAsync(Entities.Coupon coupon)
    {
        var entity = await _baseRepository.UpdateEntityAsync(coupon);
        return entity;
    }

    public async Task<bool> DeleteCouponAsync(int id)
    {
        var result = await _baseRepository.DeleteEntityAsync<Entities.Coupon>(id);

        return result;
    }
}