using System.Net;
using ApiClient.Catalog.Product.Models;
using Catalog.API.Entities;
using Catalog.API.Tests.Common;
using Catalog.API.Tests.Extensions;
using Core.Common.Helpers;
using FluentAssertions;
using IntegrationTest.Common.Configurations;
using IntegrationTest.Common.Helpers;

namespace Catalog.API.Tests.IntegrationTests.Product;

public class GetProductsTest : IClassFixture<TestWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly List<Entities.Product> _products;
    private readonly Category _category;
    private readonly SubCategory _subCategory;
    private const string Url = "/api/v1/Product";

    public GetProductsTest(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory.WithMongoDbContainer();
        
        _category = ModelHelpers.Category.GenerateCategory();
        _subCategory = ModelHelpers.SubCategory.GenerateSubCategory(categoryId: _category.Id);
        _products = ModelHelpers.Product.GenerateProductEntities(3, categoryId: _category.Id, subCategoryId: _subCategory.Id);
    } 
    
    #region Configurations

    public async Task InitializeAsync()
    {
        await _factory.StartContainersAsync();
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
    public async Task GetProducts_Success()
    {
        await EnsureDataAsync();
        var client = _factory.CreateClient();
        var response = await TestHttpRequestHelper.GetAsync(client, Url);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);

        var summaries = await HttpResponseHelpers.TransformResponseToData<List<ProductSummary>>(response, default);

        Assert.NotNull(summaries);
        Assert.True(summaries.Any());

        summaries.Count.Should().Be(_products.Count);
        
        summaries.Should().Contain(x => x.Category == _category.Name);
        summaries.Should().Contain(x => x.SubCategory == _subCategory.Name);
    }

    [Fact]
    public async Task GetProducts_Empty_Ok()
    {
        var client = _factory.CreateClient();
        var response = await TestHttpRequestHelper.GetAsync(client, Url);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var summaries = await HttpResponseHelpers.TransformResponseToData<List<ProductSummary>>(response, default);

        Assert.NotNull(summaries);
        Assert.False(summaries.Any());

        summaries.Count.Should().Be(0);
    }
}