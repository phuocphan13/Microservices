using ApiClient.Discount.Models.Coupon;
using Discount.Domain.Entities;
using Discount.Domain.Extensions;
using Discount.Domain.Repositories;

namespace Discount.Domain.Services;

public interface ICouponService
{
    Task<Coupon> GetDiscountByTextAsync(string searchText, CatalogType type);
    Task<CouponDetail> CreateDiscountAsync(CreateCouponRequestBody requestBody);
    Task<CouponDetail> UpdateDiscountAsync(UpdateCouponRequestBody requestBody);
    Task<bool> DeleteDiscountAsync(int id);
}

public class CouponService : ICouponService
{
    private readonly ICouponRepository _couponRepository;

    public CouponService(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
    }

    public async Task<Coupon> GetDiscountByTextAsync(string searchText, CatalogType type)
    {
        var coupon = await _couponRepository.GetDiscountAsync(searchText, type);

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

    public async Task<CouponDetail> CreateDiscountAsync(CreateCouponRequestBody requestBody)
    {
        var coupon = requestBody.ToCreateCoupon();

        var entity = await _couponRepository.CreateDiscountAsync(coupon);

        return entity.ToDetail();
    }

    public async Task<CouponDetail> UpdateDiscountAsync(UpdateCouponRequestBody requestBody)
    {
        var coupon = requestBody.ToUpdateCoupon();

        var entity = await _couponRepository.UpdateDiscountAsync(coupon);

        return entity.ToDetail();
    }

    public async Task<bool> DeleteDiscountAsync(int id)
    {
        var result = await _couponRepository.DeleteDiscountAsync(id);

        return result;
    }
}