using ApiClient.Catalog.Catalog;
using ApiClient.Discount.Models.Discount;
using ApiClient.Discount.Models.Discount.AmountModel;
using AutoMapper;
using Discount.Domain.Extensions;
using Discount.Domain.Repositories;
using System.Collections.Generic;
using Discount.Grpc.Protos;

namespace Discount.Domain.Services;

public interface IDiscountService
{
    Task<DiscountDetail?> GetDiscountAsync(string id);
    Task<List<DiscountDetail>?> GetListDiscountsByCatalogCodeAsync(DiscountEnum type, List<string> catalogCodes);
    Task<DiscountDetail?> GetDiscountByCatalogCodeAsync(int type, string catalogCode);
    Task<DiscountDetail?> CreateDiscountAsync(CreateDiscountRequestBody requestBody, CancellationToken cancellationToken);
    Task<DiscountDetail?> UpdateDiscountAsync(UpdateDiscountRequestBody requestBody, CancellationToken cancellationToken);
    Task<DiscountDetail?> InactiveDiscountAsync(int id);
    Task<IEnumerable<TotalAmountModel>?> TotalDiscountAmountAsync(List<CombinationCodeRequestBody> requestBody);
}

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _discountRepository;
    private readonly ICatalogApiClient _catalogApiClient;
    private readonly IMapper _mapper;

    public DiscountService(IDiscountRepository discountRepository, ICatalogApiClient catalogApiClient, IMapper mapper)
    {
        _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
        _catalogApiClient = catalogApiClient ?? throw new ArgumentNullException(nameof(catalogApiClient));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<DiscountDetail?> GetDiscountAsync(string id)
    {
        var discount = await _discountRepository.GetDiscountAsync(id);

        if (discount is null)
        {
            return null;
        }

        return discount.ToDetail();
    }

    public async Task<List<DiscountDetail>?> GetListDiscountsByCatalogCodeAsync(DiscountEnum type, List<string> catalogCodes)
    {
        var discounts = await _discountRepository.GetListDiscountsByCatalogCodeAsync(type, catalogCodes);

        if (discounts is null)
        {
            return null;
        }

        return discounts.Select(x => x.ToDetail()).ToList();
    }

    public async Task<DiscountDetail?> GetDiscountByCatalogCodeAsync(int type, string catalogCode)
    {
        var discount = await _discountRepository.GetDiscountByCatalogCodeAsync((DiscountEnum)type, catalogCode);

        if (discount is null)
        {
            return null;
        }

        return discount.ToDetail();
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
        var isValid = await ValidateDataAsync(requestBody, cancellationToken);

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
        var isValid = await ValidateDataAsync(requestBody, cancellationToken);

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
    private async Task<bool> ValidateDataAsync<T>(T requestBody, CancellationToken cancellationToken)
        where T : BaseDiscountRequestBody
    {
        var isDateValid = await ValidationDateAsync(requestBody);

        var isCatalogCodeExisted = await ValidateCatalogCodeExistedAsync(requestBody.CatalogCode!, requestBody.Type!.Value, cancellationToken);

        return isDateValid && isCatalogCodeExisted;
    }

    private async Task<bool> ValidationDateAsync<T>(T requestBody)
        where T : BaseDiscountRequestBody
    {
        var isOverlap = await _discountRepository.AnyDateAsync(requestBody.CatalogCode!, requestBody.Type!.Value, requestBody.FromDate, requestBody.ToDate);

        return !isOverlap;
    }

    private async Task<bool> ValidateCatalogCodeExistedAsync(string catalogCode, DiscountEnum type, CancellationToken cancellationToken)
    {
        var result = await _catalogApiClient.ValidateCatalogCodeAsync(catalogCode, type, cancellationToken);

        return result.IsSuccessStatusCode;
    }
    #endregion

    public async Task<IEnumerable<TotalAmountModel>?> TotalDiscountAmountAsync(List<CombinationCodeRequestBody> requestBody)
    {  
        // ABC.CA.casd
        // ListCodeRequest --> Code: "ProductCode.SubCategoryCode.CategoryCode"
        var productCodes = new List<string>();

        foreach (var item in requestBody)
        {
            string[] codes = item.CombineCode.Split('.'); // [111, 444, 555] 

            productCodes.AddRange(codes);
        }

        var discounts = await _discountRepository.GetAmountDiscountAsync(productCodes);

        if (discounts is null || !discounts.Any())
        {
            return null;
        }

        // totalAmounts --> List Product Codes based on RequestBody
        var totalAmounts = requestBody.Select(x => new TotalAmountModel()
        {
            CatalogCode = x.CombineCode.Split(".")[0],
            Amount = 0
        });
        
        // Item -> "A.B.C"
        // Item -> "ProductCode.SubCategoryCode.CategoryCode"
        // Discounts: A, C
        
        // DiscountItems --> A,C
        
        
        foreach (var item in requestBody)
        {
            // Assume: if has any Discounts for this item
            
            // Get ProductCode in totalAmounts => increase Amount
            var codeArrays = item.CombineCode.Split('.');
            var product = totalAmounts.First(x => x.CatalogCode == codeArrays[0]);

            var discountItems = discounts.Where(x => codeArrays.Contains(x.CatalogCode));
            product.Amount += discountItems.Sum(x => x.Amount);
        }

        return totalAmounts.Where(x => x.Amount > 0);
    }
}