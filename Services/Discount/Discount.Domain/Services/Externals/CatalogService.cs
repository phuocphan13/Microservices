using ApiClient.Catalog.Validation;

namespace Discount.Domain.Services.Externals;

public interface ICatalogService
{
    Task<bool> ValidateCatalogCodeExistedAsync(string catalogCode, DiscountEnum type, CancellationToken cancellationToken = default);
}

public class CatalogService : ICatalogService
{
    private readonly IValidationApiClient _validationApiClient;

    public CatalogService(IValidationApiClient validationApiClient)
    {
        _validationApiClient = validationApiClient;
    }


    public async Task<bool> ValidateCatalogCodeExistedAsync(string catalogCode, DiscountEnum type, CancellationToken cancellationToken)
    {
        var result = await _validationApiClient.ValidateCatalogCodeAsync(catalogCode, type, cancellationToken);

        return result.IsSuccessCode;
    }
}