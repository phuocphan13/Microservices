using ApiClient.Discount.Models.Coupon;
using Discount.Domain.Entities;
using Discount.Domain.Extensions;
using Discount.Domain.Repositories;

namespace Discount.Domain.Services;

public interface IDiscountService
{
    Task<Coupon> GetDiscountByTextAsync(string searchText, CatalogType type);
    Task<CouponDetail> CreateDiscountAsync(CreateCouponRequestBody requestBody);
    Task<CouponDetail> UpdateDiscountAsync(UpdateCouponRequestBody requestBody);
    Task<bool> DeleteDiscountAsync(int id);
}

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _discountRepository;

    public DiscountService(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<Coupon> GetDiscountByTextAsync(string searchText, CatalogType type)
    {
        var coupon = await _discountRepository.GetDiscountAsync(searchText, type);

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

        var entity = await _discountRepository.CreateDiscountAsync(coupon);

        return entity.ToDetail();
    }

    public async Task<CouponDetail> UpdateDiscountAsync(UpdateCouponRequestBody requestBody)
    {
        var coupon = requestBody.ToUpdateCoupon();

        var entity = await _discountRepository.UpdateDiscountAsync(coupon);

        return entity.ToDetail();
    }

    public async Task<bool> DeleteDiscountAsync(int id)
    {
        var result = await _discountRepository.DeleteDiscountAsync(id);

        return result;
    }
}