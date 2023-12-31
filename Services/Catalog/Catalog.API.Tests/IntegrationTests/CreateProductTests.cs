using System.Net;
using ApiClient.Catalog.Product.Models;
using ApiClient.Common;
using Catalog.API.Entities;
using Catalog.API.Tests.Common;
using Catalog.API.Tests.Extensions;
using Core.Common.Api;
using Core.Common.Helpers;
using IntegrationTest.Common.Configurations;
using FluentAssertions;
using IntegrationTest.Common.Common;
using Newtonsoft.Json;

namespace Catalog.API.Tests.IntegrationTests;

public class CreateProductTests : IClassFixture<TestWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly Category _category;
    private readonly SubCategory _subCategory;

    public CreateProductTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory.WithMongoDbContainer();
        _client = _factory.CreateClient(ConfigConst.ConfigFactoryClientOptions(ConfigurationConst.CatalogApiName));

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
    public async Task CreateProduct_Success()
    {
        var requestBody = ModelHelpers.Product.GenerateCreateRequestBody(initAction: x =>
        {
            x.Category = _category.Name;
            x.SubCategory = _subCategory.Name;
        });

        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/v1/Product");
        httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");
        // Act
        var response = await _client.SendAsync(httpRequestMessage);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        
        var result = await HttpResponseHelpers.TransformResponseToData<ApiDataResult<ProductDetail>>(response, default);

        Assert.NotNull(result);
        Assert.NotNull(result.Data);

        result.Data.Name.Should().Be(requestBody.Name);
        result.Data.Category.Should().Be(_category.Name);
        result.Data.SubCategory.Should().Be(_subCategory.Name);
    }
}