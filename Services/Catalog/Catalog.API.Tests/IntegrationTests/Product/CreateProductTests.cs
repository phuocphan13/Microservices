using System.Net;
using ApiClient.Catalog.Product.Models;
using Catalog.API.Common.Consts;
using Catalog.API.Entities;
using Catalog.API.Tests.Common;
using Catalog.API.Tests.Extensions;
using IntegrationTest.Common.Configurations;
using FluentAssertions;
using IntegrationTest.Common.Helpers;
using Platform.ApiBuilder;
using Platform.ApiBuilder.Helpers;

namespace Catalog.API.Tests.IntegrationTests.Product;

public class CreateProductTests : IClassFixture<TestWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly Category _category;
    private readonly SubCategory _subCategory;
    private const string Url = "/api/v1/Product";

    public CreateProductTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory.WithMongoDbContainer();

        _category = ModelHelpers.Category.GenerateCategory();
        _subCategory = ModelHelpers.SubCategory.GenerateSubCategory(categoryId: _category.Id);
    }

    #region Configurations

    public async Task InitializeAsync()
    {
        await _factory.StartContainersAsync();
        
        await _factory.EnsureCreatedAndPopulateSingleDataAsync(_category);
        await _factory.EnsureCreatedAndPopulateSingleDataAsync(_subCategory);
    }

    public async Task DisposeAsync()
    {
        await _factory.StopContainersAsync();
    }

    #endregion

    [Fact]
    public async Task CreateProduct_Ok()
    {
        var requestBody = ModelHelpers.Product.GenerateCreateRequestBody(initAction: x =>
        {
            x.CategoryId = _category.Id;
            x.SubCategoryId = _subCategory.Id;
        });

        var client = _factory.CreateClient();
        var response = await TestHttpRequestHelper.PostAsync(requestBody, client, Url);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        
        var result = await HttpResponseHelpers.TransformResponseToData<ApiDataResult<ProductDetail>>(response, default);

        Assert.NotNull(result);
        Assert.NotNull(result.Result);

        result.Result.Name.Should().Be(requestBody.Name);
        result.Result.Category.Should().Be(_category.Name);
        result.Result.SubCategory.Should().Be(_subCategory.Name);
    }

    [Fact]
    public async Task CreateProduct_NotFoundCategory()
    {
        var requestBody = ModelHelpers.Product.GenerateCreateRequestBody();
        var expectedMessage = ResponseMessages.Product.PropertyNotExisted("Category Id", requestBody.CategoryId);

        // Act
        var client = _factory.CreateClient();
        var response = await TestHttpRequestHelper.PostAsync(requestBody, client, Url);

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);

        var result = await HttpResponseHelpers.TransformResponseToData<ApiDataResult<ProductDetail>>(response, default);

        Assert.NotNull(result);
        result.Message.Should().Be(expectedMessage);
    }

    [Fact]
    public async Task CreateProduct_NotFoundSubCategory()
    {
        var requestBody = ModelHelpers.Product.GenerateCreateRequestBody(initAction: x =>
        {
            x.CategoryId = _category.Id;
        });
        
        var expectedMessage = ResponseMessages.Product.PropertyNotExisted("SubCategory Id", requestBody.SubCategoryId);

        // Act
        var client = _factory.CreateClient();
        var response = await TestHttpRequestHelper.PostAsync(requestBody, client, Url);

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);

        var result = await HttpResponseHelpers.TransformResponseToData<ApiDataResult<ProductDetail>>(response, default);

        Assert.NotNull(result);
        result.Message.Should().Be(expectedMessage);
    }
}