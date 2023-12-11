using ApiClient.Catalog.Models.Product;
using ApiClient.Common;
using Microsoft.Extensions.Configuration;

namespace ApiClient.Catalog.ApiClient;

public interface ICatalogApiClient
{
    Task<ApiDataResult<List<ProductSummary>>> GetProducts(CancellationToken cancellationToken = default);
    Task<ApiDataResult<ProductDetail>> GetProductByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<ApiDataResult<List<ProductSummary>>> GetProductByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<ApiDataResult<ProductDetail>> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ApiDataResult<ProductDetail>> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ApiStatusResult> DeleteProductAsync(string id, CancellationToken cancellationToken = default);
}

public class CatalogApiClient : CommonApiClient, ICatalogApiClient
{
    public CatalogApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        : base(httpClientFactory, configuration)
    {
    }

    public async Task<ApiDataResult<List<ProductSummary>>> GetProducts(CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetProducts}";

        var result = await GetAsync<List<ProductSummary>>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<ProductDetail>> GetProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetProductById}";

        url = url.AddQueryStringParameter("id", id, true);

        var result = await GetAsync<ProductDetail>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<List<ProductSummary>>> GetProductByCategoryAsync(string category, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetProductByCategory}";

        url = url.AddDataInUrl(nameof(category), category);

        var result = await GetAsync<List<ProductSummary>>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<ProductDetail>> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.CreateProduct}";


        var result = await PostAsync<CreateProductRequestBody, ProductDetail>(url, requestBody, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<ProductDetail>> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.UpdateProduct}";

        var result = await PutAsync<UpdateProductRequestBody, ProductDetail>(url, requestBody, cancellationToken);

        return result;
    }

    public async Task<ApiStatusResult> DeleteProductAsync(string id, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.DeleteProduct}";

        var result = await DeleteAsync(url, cancellationToken);

        return result;
    }
}