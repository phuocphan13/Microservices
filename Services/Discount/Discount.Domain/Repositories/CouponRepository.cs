using Discount.Domain.Repositories.Common;

namespace Discount.Domain.Repositories;

public interface ICouponRepository
{
    Task<Entities.Coupon?> GetCouponAsync(string id);
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

    public async Task<Entities.Coupon?> GetCouponAsync(string id)
    {
        const string query = "Id = @Id";

        var coupon = await _baseRepository.QueryFirstOrDefaultAsync<Entities.Coupon>(query, new { Id = int.Parse(id) });

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