using ApiClient.Discount.Models.Coupon;
using Discount.Domain.Entities;
using Discount.Domain.Extensions;
using Discount.Domain.Repositories;

namespace Discount.Domain.Services;

public interface ICouponService
{
    Task<Coupon> GetCouponByTextAsync(string searchText, CatalogType type);
    Task<CouponDetail> CreateCouponAsync(CreateCouponRequestBody requestBody);
    Task<CouponDetail> UpdateCouponAsync(UpdateCouponRequestBody requestBody);
    Task<bool> DeleteCouponAsync(int id);
}

public class CouponService : ICouponService
{
    private readonly ICouponRepository _couponRepository;

    public CouponService(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
    }

    public async Task<Coupon> GetCouponByTextAsync(string searchText, CatalogType type)
    {
        var coupon = await _couponRepository.GetCouponAsync(searchText, type);

        if (coupon is null)
        {
            //ToDo Create Summary/Detail Coupon
            return new Coupon()
            {
                Amount = 0,
                Description = "No Discount Desc"
            };
        }

        return coupon;
    }

    public async Task<CouponDetail> CreateCouponAsync(CreateCouponRequestBody requestBody)
    {
        var coupon = requestBody.ToCreateCoupon();

        var entity = await _couponRepository.CreateCouponAsync(coupon);

        return entity.ToDetail();
    }

    public async Task<CouponDetail> UpdateCouponAsync(UpdateCouponRequestBody requestBody)
    {
        var coupon = requestBody.ToUpdateCoupon();

        var entity = await _couponRepository.UpdateCouponAsync(coupon);

        return entity.ToDetail();
    }

    public async Task<bool> DeleteCouponAsync(int id)
    {
        var result = await _couponRepository.DeleteCouponAsync(id);

        return result;
    }
}