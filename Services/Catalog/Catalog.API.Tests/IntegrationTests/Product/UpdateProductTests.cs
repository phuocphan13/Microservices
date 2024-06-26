using System.Net;
using ApiClient.Catalog.Product.Models;
using Catalog.API.Common.Consts;
using Catalog.API.Entities;
using Catalog.API.Tests.Common;
using Catalog.API.Tests.Extensions;
using FluentAssertions;
using IntegrationTest.Common.Configurations;
using IntegrationTest.Common.Helpers;
using Platform.ApiBuilder;
using Platform.ApiBuilder.Helpers;

namespace Catalog.API.Tests.IntegrationTests.Product;

public class UpdateProductTests : IClassFixture<TestWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly Entities.Product _product;
    private readonly Category _category;
    private readonly SubCategory _subCategory;

    private readonly Category _categoryUpdated;
    private readonly SubCategory _subCategoryUpdated;
    private const string Url = "/api/v1/Product";

    public UpdateProductTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory.WithMongoDbContainer();

        _category = ModelHelpers.Category.GenerateCategory();
        _subCategory = ModelHelpers.SubCategory.GenerateSubCategory(categoryId: _category.Id);
        _product = ModelHelpers.Product.GenerateProductEntity(categoryId: _category.Id, subCategoryId: _subCategory.Id);

        _categoryUpdated = ModelHelpers.Category.GenerateCategory();
        _subCategoryUpdated = ModelHelpers.SubCategory.GenerateSubCategory(categoryId: _categoryUpdated.Id);
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


        await _factory.EnsureCreatedAndPopulateSingleDataAsync(_categoryUpdated);
        await _factory.EnsureCreatedAndPopulateSingleDataAsync(_subCategoryUpdated);
    }

    #endregion

    [Fact]
    public async Task UpdateProduct_Ok()
    {
        var requestBody = ModelHelpers.Product.GenerateUpdateRequestBody(initAction: x =>
        {
            x.Id = _product.Id;
            x.CategoryId = _categoryUpdated.Id;
            x.SubCategoryId = _subCategoryUpdated.Id;
        });
        
        var client = _factory.CreateClient();
        var response = await TestHttpRequestHelper.PutAsync(requestBody, client, Url);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);

        var result = await HttpResponseHelpers.TransformResponseToData<ApiDataResult<ProductDetail>>(response, default);

        Assert.NotNull(result);
        Assert.NotNull(result.Result);

        result.Result.Name.Should().Be(requestBody.Name);
        result.Result.Category.Should().Be(_categoryUpdated.Name);
        result.Result.SubCategory.Should().Be(_subCategoryUpdated.Name);
    }

    [Fact]
    public async Task UpdateProduct_NotFoundCategory()
    {
        var requestBody = ModelHelpers.Product.GenerateUpdateRequestBody(initAction: x =>
        {
            x.Id = _product.Id;
        });
        
        var expectedMessage = ResponseMessages.Product.PropertyNotExisted("Category Id", requestBody.CategoryId);

        // Act
        var client = _factory.CreateClient();
        var response = await TestHttpRequestHelper.PutAsync(requestBody, client, Url);

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);

        var result = await HttpResponseHelpers.TransformResponseToData<ApiDataResult<ProductDetail>>(response, default);

        Assert.NotNull(result);
        result.Message.Should().Be(expectedMessage);
    }

    [Fact]
    public async Task UpdateProduct_NotFoundSubCategory()
    {
        var requestBody = ModelHelpers.Product.GenerateUpdateRequestBody(initAction: x =>
        {
            x.Id = _product.Id;
            x.CategoryId = _categoryUpdated.Id;
        });

        var expectedMessage = ResponseMessages.Product.PropertyNotExisted("SubCategory Id", requestBody.SubCategoryId);

        // Act
        var client = _factory.CreateClient();
        var response = await TestHttpRequestHelper.PutAsync(requestBody, client, Url);

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);

        var result = await HttpResponseHelpers.TransformResponseToData<ApiDataResult<ProductDetail>>(response, default);

        Assert.NotNull(result);
        result.Message.Should().Be(expectedMessage);
    }
}