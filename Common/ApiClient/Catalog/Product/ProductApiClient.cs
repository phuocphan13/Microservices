using ApiClient.Catalog.Product.Models;
using ApiClient.Common;
using Microsoft.Extensions.Configuration;
using Platform.ApiBuilder;

namespace ApiClient.Catalog.Product;

public interface IProductApiClient
{
    Task<ApiCollectionResult<ProductSummary>> GetProducts(CancellationToken cancellationToken = default);
    Task<ApiDataResult<ProductDetail>> GetProductByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<ApiCollectionResult<ProductSummary>> GetProductByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<ApiDataResult<ProductDetail>> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ApiDataResult<ProductDetail>> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ApiStatusResult> DeleteProductAsync(string id, CancellationToken cancellationToken = default);
}

public class ProductApiClient : CommonApiClient, IProductApiClient
{
    public ProductApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        : base(httpClientFactory, configuration)
    {
    }

    public async Task<ApiCollectionResult<ProductSummary>> GetProducts(CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetProducts}";

        var result = await GetCollectionAsync<ProductSummary>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<ProductDetail>> GetProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetProductById}";

        url = url.AddQueryStringParameter("id", id, true);

        var result = await GetSingleAsync<ProductDetail>(url, cancellationToken);

        return result;
    }

    public async Task<ApiCollectionResult<ProductSummary>> GetProductByCategoryAsync(string category, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetProductByCategory}";

        url = url.AddDataInUrl(nameof(category), category);

        var result = await GetCollectionAsync<ProductSummary>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<ProductDetail>> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.CreateProduct}";

        var result = await PostAsync<ProductDetail>(url, requestBody, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<ProductDetail>> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.UpdateProduct}";

        var result = await PutAsync<ProductDetail>(url, requestBody, cancellationToken);

        return result;
    }

    public async Task<ApiStatusResult> DeleteProductAsync(string id, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.DeleteProduct}";

        var result = await DeleteAsync(url, cancellationToken);

        return result;
    }
}