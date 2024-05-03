using System.Net;
using ApiClient.Catalog.Product.Models;
using Catalog.API.Entities;
using Catalog.API.Tests.Common;
using Catalog.API.Tests.Extensions;
using FluentAssertions;
using IntegrationTest.Common.Configurations;
using IntegrationTest.Common.Helpers;
using Platform.ApiBuilder.Helpers;
using UnitTest.Common.Helpers;

namespace Catalog.API.Tests.IntegrationTests.Product;

public class GetProductByCategoryTests : IClassFixture<TestWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly List<Entities.Product> _products;
    private readonly Category _category;
    private readonly SubCategory _subCategory;
    private readonly string Url;

    public GetProductByCategoryTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory.WithMongoDbContainer();

        _category = ModelHelpers.Category.GenerateCategory();
        _subCategory = ModelHelpers.SubCategory.GenerateSubCategory(categoryId: _category.Id);
        _products = ModelHelpers.Product.GenerateProductEntities(3, categoryId: _category.Id, subCategoryId: _subCategory.Id);

        Url = $"/api/v1/Product/GetProductByCategory/{_category.Name}";
    }

    #region Configurations

    public async Task InitializeAsync()
    {
        await _factory.StartContainersAsync();
        await EnsureDataAsync();
    }

    public async Task DisposeAsync()
    {
        await _factory.StopContainersAsync();
    }

    private async Task EnsureDataAsync()
    {
        await _factory.EnsureCreatedAndPopulateSingleDataAsync(_category);
        await _factory.EnsureCreatedAndPopulateSingleDataAsync(_subCategory);
        await _factory.EnsureCreatedAndPopulateDataAsync(_products);
    }

    #endregion

    [Fact]
    public async Task GetProductByCategory_Ok()
    {
        var client = _factory.CreateClient();
        var response = await TestHttpRequestHelper.GetAsync(client, Url);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);

        var summaries = await HttpResponseHelpers.TransformResponseToData<List<ProductSummary>>(response, default);

        Assert.NotNull(summaries);
        
        summaries.Count.Should().Be(_products.Count);
    }

    [Fact]
    public async Task GetProductByCategory_NotFound()
    {
        string urlTemp = $"/api/v1/Product/GetProductByCategory/{CommonHelpers.GenerateBsonId()}";

        var client = _factory.CreateClient();
        var response = await TestHttpRequestHelper.GetAsync(client, urlTemp);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
