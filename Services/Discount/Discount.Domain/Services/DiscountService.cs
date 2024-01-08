using ApiClient.Discount.Models.Discount;
using Discount.Domain.Extensions;
using Discount.Domain.Repositories;
using Discount.Domain.Services.Externals;

namespace Discount.Domain.Services;

public interface IDiscountService
{
    Task<DiscountDetail?> CreateDiscountAsync(CreateDiscountRequestBody requestBody, CancellationToken cancellationToken);
    Task<DiscountDetail?> UpdateDiscountAsync(UpdateDiscountRequestBody requestBody, CancellationToken cancellationToken);
    Task<DiscountDetail?> InactiveDiscountAsync(int id);
}

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _discountRepository;
    private readonly ICatalogService _catalogService;

    public DiscountService(IDiscountRepository discountRepository, ICatalogService catalogService)
    {
        _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
        _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
    }

    public async Task<DiscountDetail?> CreateDiscountAsync(CreateDiscountRequestBody requestBody, CancellationToken cancellationToken)
    {
        if (requestBody is null)
        {
            throw new ArgumentNullException(nameof(requestBody));
        }

        if (requestBody.Amount is null)
        {
            throw new ArgumentNullException(nameof(requestBody.Amount));
        }

        if (string.IsNullOrWhiteSpace(requestBody.CatalogCode))
        {
            throw new ArgumentNullException(nameof(requestBody.CatalogCode));
        }

        if (requestBody.Type is null)
        {
            throw new ArgumentNullException(nameof(requestBody.Type));
        }

        if (requestBody.FromDate is null)
        {
            throw new ArgumentNullException(nameof(requestBody.FromDate));
        }

        if (requestBody.ToDate is null)
        {
            throw new ArgumentNullException(nameof(requestBody.ToDate));
        }

        return await CreateDiscountInternalAsync(requestBody, cancellationToken);
    }

    public async Task<DiscountDetail?> UpdateDiscountAsync(UpdateDiscountRequestBody requestBody, CancellationToken cancellationToken)
    {
        if (requestBody is null)
        {
            throw new ArgumentNullException(nameof(requestBody));
        }

        if (requestBody.Id is null)
        {
            throw new ArgumentNullException(nameof(requestBody.Id));
        }

        if (requestBody.Amount is null)
        {
            throw new ArgumentNullException(nameof(requestBody.Amount));
        }

        if (string.IsNullOrWhiteSpace(requestBody.CatalogCode))
        {
            throw new ArgumentNullException(nameof(requestBody.CatalogCode));
        }

        if (requestBody.Type is null)
        {
            throw new ArgumentNullException(nameof(requestBody.Type));
        }

        if (requestBody.FromDate is null)
        {
            throw new ArgumentNullException(nameof(requestBody.FromDate));
        }

        if (requestBody.ToDate is null)
        {
            throw new ArgumentNullException(nameof(requestBody.ToDate));
        }

        return await UpdateDiscountInternalAsync(requestBody, cancellationToken);
    }

    private async Task<DiscountDetail?> CreateDiscountInternalAsync(CreateDiscountRequestBody requestBody, CancellationToken cancellationToken)
    {
        var isValid = await ValidationDateAsync(requestBody, cancellationToken);

        if (!isValid)
        {
            return null;
        }

        var discount = requestBody.ToEntityFromCreateBody();

        var entity = await _discountRepository.CreateDiscountAsync(discount);

        return entity.ToDetail();
    }

    private async Task<DiscountDetail?> UpdateDiscountInternalAsync(UpdateDiscountRequestBody requestBody, CancellationToken cancellationToken)
    {
        var isValid = await ValidationDateAsync(requestBody, cancellationToken);
        
        if (!isValid)
        {
            return null;
        }

        var discount = requestBody.ToEntityFromUpdateBody();

        var entity = await _discountRepository.UpdateDiscountAsync(discount);

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

    private async Task<bool> ValidationDateAsync<T>(T requestBody, CancellationToken cancellationToken)
        where T : BaseDiscountRequestBody
    {
        var isExisted = await _catalogService.ValidateCatalogCodeExistedAsync(requestBody.CatalogCode!, requestBody.Type!.Value, cancellationToken);

        var isOverlap = await _discountRepository.AnyDateAsync(requestBody.CatalogCode!, requestBody.Type!.Value, requestBody.FromDate, requestBody.ToDate);

        return isExisted && !isOverlap;
    }

    #endregion
}