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
    private readonly IDiscountEntityRepository _discountEntityRepository;
    private readonly IMapper _mapper;

    public DiscountService(IDiscountEntityRepository discountEntityRepository, IMapper mapper)
    {
        _discountEntityRepository = discountEntityRepository ?? throw new ArgumentNullException(nameof(discountEntityRepository));
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

        var entity = await _discountEntityRepository.CreateDiscountAsync(version);

        return entity.ToDetail();
    }

    public async Task<DiscountDetail?> InactiveDiscountAsync(int id)
    {
        var discountCurrentVersion = await _discountEntityRepository.GetDiscountAsync<DiscountVersion>(id.ToString());

        if (discountCurrentVersion is null)
        {
            return null;
        }

        var history = _mapper.Map<DiscountHistory>(discountCurrentVersion);
        await _discountEntityRepository.CreateDiscountAsync(history);
        await _discountEntityRepository.DeleteDiscountAsync<DiscountVersion>(discountCurrentVersion.Id);

        return discountCurrentVersion.ToDetail();
    }
}