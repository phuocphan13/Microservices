using ApiClient.Discount.Models.Discount;
using AutoMapper;
using Discount.Domain.Entities;
using Discount.Domain.Extensions;
using Discount.Domain.Repositories;

namespace Discount.Domain.Services;

public interface IDiscountService
{
    Task<DiscountDetail?> CreateDiscountVersionAsync(CreateDiscountRequestBody requestBody);
    Task<DiscountDetail?> InactiveDiscountAsync(int id);
}

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;

    public DiscountService(IDiscountRepository discountRepository, IMapper mapper)
    {
        _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<DiscountDetail?> CreateDiscountVersionAsync(CreateDiscountRequestBody requestBody)
    {
        //ToDo: Check null and Add Message
        if (string.IsNullOrWhiteSpace(requestBody.CouponId))
        {
            return null;
        }

        var version = requestBody.ToCreateDiscountVersion(requestBody.CouponId);

        var entity = await _discountRepository.CreateDiscountAsync(version);

        return entity.ToDetail();
    }

    public async Task<DiscountDetail?> InactiveDiscountAsync(int id)
    {
        var discount = await _discountRepository.GetDiscountAsync(id.ToString());

        if (discount is null)
        {
            return null;
        }

        discount.IsActive = false;
        discount = await _discountRepository.UpdateDiscountAsync(discount);

        return discount.ToDetail();
    }

    #region Internal Function

    // private async Task<bool> ValidationDateAsync<T>(T requestBody)
    //     where T : BaseDiscountRequestBody
    // {
    //     var isValid = await _discountRepository.
    // }

    #endregion
}