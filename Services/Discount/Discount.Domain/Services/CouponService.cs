using ApiClient.Discount.Models.Coupon;
using Discount.Domain.Extensions;
using Discount.Domain.Repositories;

namespace Discount.Domain.Services;

public interface ICouponService
{
    Task<CouponDetail?> GetCouponAsync(string id);
    Task<CouponDetail> CreateCouponAsync(CreateCouponRequestBody requestBody);
    Task<CouponDetail> GetCouponForCreateAsync(string name);
    Task<CouponDetail> UpdateCouponAsync(UpdateCouponRequestBody requestBody);
    Task<bool> InactiveCoupon(int id);
}

public class CouponService : ICouponService
{
    private readonly ICouponRepository _couponRepository;

    public CouponService(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
    }

    public async Task<CouponDetail?> GetCouponAsync(string id)
    {
        var coupon = await _couponRepository.GetCouponAsync(id);

        if (coupon is null)
        {
            return null;
        }

        return coupon.ToDetail();
    }

    public async Task<CouponDetail> CreateCouponAsync(CreateCouponRequestBody requestBody)
    {
        var coupon = requestBody.ToCreateCoupon();

        var entity = await _couponRepository.CreateCouponAsync(coupon);

        return entity.ToDetail();
    }

    public async Task<CouponDetail> GetCouponForCreateAsync(string name)
    {
        var coupon = await _couponRepository.GetCouponAsync(name);

        if (coupon is null)
        {
            return new CouponDetail()
            {
                Description = "No Discount Desc"
            };
        }

        return coupon.ToDetail();
    }

    public async Task<CouponDetail> UpdateCouponAsync(UpdateCouponRequestBody requestBody)
    {
        var data = await _couponRepository.GetCouponAsync(requestBody.Id.ToString());

        data!.ToUpdateCoupon(requestBody);

        var entity = await _couponRepository.UpdateCouponAsync(data!);

        return entity.ToDetail();
    }

    public async Task<bool> InactiveCoupon(int id)
    {
        var result = await _couponRepository.InactiveCouponAsync(id);

        return result;
    }
}