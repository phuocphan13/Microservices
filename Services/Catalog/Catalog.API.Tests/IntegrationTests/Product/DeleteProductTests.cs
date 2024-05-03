using System.Net;
using Catalog.API.Entities;
using Catalog.API.Tests.Common;
using Catalog.API.Tests.Extensions;
using FluentAssertions;
using IntegrationTest.Common.Configurations;
using IntegrationTest.Common.Helpers;

namespace Catalog.API.Tests.IntegrationTests.Product;

public class DeleteProductTests : IClassFixture<TestWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly Entities.Product _product;
    private readonly Category _category;
    private readonly SubCategory _subCategory;
    private readonly string Url;

    public DeleteProductTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory.WithMongoDbContainer();

        _category = ModelHelpers.Category.GenerateCategory();
        _subCategory = ModelHelpers.SubCategory.GenerateSubCategory(categoryId: _category.Id);
        _product = ModelHelpers.Product.GenerateProductEntity(categoryId: _category.Id, subCategoryId: _subCategory.Id);
        
        Url = $"/api/v1/Product/{_product.Id}";
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
        await _factory.EnsureCreatedAndPopulateSingleDataAsync(_product);
    }

    #endregion

    [Fact]
    public async Task DeleteProductById_Ok()
    {
        var client = _factory.CreateClient();
        var response = await TestHttpRequestHelper.DeleteAsync(client, Url);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task DeleteProductById_MethodNotAllowed()
    {
        string id = " ";
        string urlTemp = $"/api/v1/Product/{id}";
        
        var client = _factory.CreateClient();
        var response = await TestHttpRequestHelper.DeleteAsync(client, urlTemp);

        response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
    }
}