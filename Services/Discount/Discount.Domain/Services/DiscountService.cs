using ApiClient.Catalog.Catalog;
using ApiClient.Discount.Models.Discount;
using ApiClient.Discount.Models.Discount.AmountModel;
using AutoMapper;
using Discount.Domain.Extensions;
using Discount.Domain.Repositories;
using System.Collections.Generic;

namespace Discount.Domain.Services;

public interface IDiscountService
{
    Task<DiscountDetail?> GetDiscountAsync(string id);
    Task<List<DiscountDetail>?> GetListDiscountsByCatalogCodeAsync(DiscountEnum type, List<string> catalogCodes);
    Task<DiscountDetail?> GetDiscountByCatalogCodeAsync(int type, string catalogCode);
    Task<DiscountDetail?> CreateDiscountAsync(CreateDiscountRequestBody requestBody, CancellationToken cancellationToken);
    Task<DiscountDetail?> UpdateDiscountAsync(UpdateDiscountRequestBody requestBody, CancellationToken cancellationToken);
    Task<DiscountDetail?> InactiveDiscountAsync(int id);
    Task<List<DiscountDetail>?> AmountDiscountAsync (AmountDiscountRequestBody requestBody, CancellationToken cancellationToken);
    Task<List<DiscountDetail>?> TotalDiscountAmountAsync(List<ListCodeRequestBody> requestBody, CancellationToken cancellationToken);
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

    public async Task<List<DiscountDetail>?> AmountDiscountAsync(AmountDiscountRequestBody requestBody, CancellationToken cancellationToken)
    {
        //bien ra AmountDiscountRepositoryModel
        var cateCodes = requestBody.Categories.Select(x => x.CatalogCode);
        var subCateCodes = requestBody.Categories.SelectMany(x => x.SubCategories).Select(a => a.CatalogCode);
        var prodCateCodes = requestBody.Categories.SelectMany(x => x.SubCategories).SelectMany(a => a.Products).Select(b => b.CatalogCode);

        var repoModel = new List<AmountDiscountRepositoryModel>();

        repoModel = new List<AmountDiscountRepositoryModel>()
        {
            new AmountDiscountRepositoryModel()
            {
                Type = "2",
                CatalogCodes = cateCodes.ToList()
            },
            new AmountDiscountRepositoryModel()
            {
                Type = "3",
                CatalogCodes = subCateCodes.ToList()
            },
            new AmountDiscountRepositoryModel()
            {
                Type = "4",
                CatalogCodes = prodCateCodes.ToList()
            }
        };
        var response = await _discountRepository.AmountDiscountAsync(repoModel);
        return response.Select(x => x.ToDetail()).ToList();
    }

    public async Task<List<DiscountDetail>?> TotalDiscountAmountAsync( List<ListCodeRequestBody> requestBody, CancellationToken cancellationToken)
    {
        var productCode = new List<string>();

        List<string[]> codeStr = new List<string[]>();

        foreach ( var item in requestBody)
        {
            string[] codes = item.CodeStr.Split('.'); // [ 111, 444, 555] 

            foreach (var code in codes)
            {
                productCode.Add(code); //[ 111, 444, 555, 666, 4644, 888,..., n ]
            }    
        }    

        var response = await _discountRepository.GetAmountDiscountAsync(productCode);

        return response.Select(x => x.ToDetail()).ToList();
    }
}