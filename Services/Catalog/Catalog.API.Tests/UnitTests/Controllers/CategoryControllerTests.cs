using ApiClient.Catalog.Category.Models;
using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Catalog.Product.Models;
using ApiClient.Common;
using Catalog.API.Controllers;
using Catalog.API.Extensions;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Runtime.CompilerServices;
using UnitTest.Common.Helpers;
using ModelHelpers = Catalog.API.Tests.Common.ModelHelpers;

namespace Catalog.API.Tests.UnitTests.Controllers;

public class CategoryControllerTests
{
    //GetCategories
    #region
    [Fact]
    public async Task GetCategories_ValidParams_ExpectedResult()
    {
        var categorySummarise = ModelHelpers.Category.GenerateCategorySummaries();

        var categoryService = new Mock<ICategoryService>();
        categoryService.Setup(x => x.GetCategoriesAsync(default)).ReturnsAsync(CommonHelpers.ApiResult.Ok(categorySummarise));
        var controller = new CategoryController(categoryService.Object);

        var result = await controller.GetCategories(default);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);

        var data = Assert.IsType<ApiDataResult<List<CategorySummary>>>(okResult.Value);

        Assert.NotNull(data.Data);
        Assert.Equal(data.Data.Count, categorySummarise.Count);
    }

    [Fact]
    public async Task GetCategories_ValidParams_NotFound()
    {
        var categoryService = new Mock<ICategoryService>();
        categoryService.Setup(x => x.GetCategoriesAsync(default)).ReturnsAsync((ApiDataResult<List<CategorySummary>>)null!);

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.GetCategories(default);

        Assert.IsType<NotFoundResult>(result);
    }
    #endregion
    //GetCategoryById
    #region
    [Fact]
    public async Task GetCategoryById_ValidParams_ExpectedResult()
    {
        var categoryDetail = ModelHelpers.Category.GenerateCategory().ToDetail();

        var categoryService = new Mock<ICategoryService>();
        categoryService.Setup(x => x.GetCategoryByIdAsync(categoryDetail.Id!, default)).ReturnsAsync(CommonHelpers.ApiResult.Ok(categoryDetail));

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.GetCategoryById(categoryDetail.Id!, default);

        OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);

        var data = Assert.IsType<ApiDataResult<CategoryDetail>>(okObjectResult.Value);

        Assert.NotNull(data);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public async Task GetCategoryById_InvalidParams_BadRequest(string id)
    {
        var categoryService = new Mock<ICategoryService>();

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.GetCategoryById(id, default);

        Assert.True(result is BadRequestResult || result is BadRequestObjectResult);
    }

    [Fact]
    public async Task GetCategoryById_ValidParams_NotFound()
    {
        var id = CommonHelpers.GenerateBsonId();

        var categoryService = new Mock<ICategoryService>();

        categoryService.Setup(x => x.GetCategoryByIdAsync(id, default)).ReturnsAsync((ApiDataResult<CategoryDetail>)null!);

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.GetCategoryById(id, default);

        Assert.True(result is NotFoundResult || result is NotFoundObjectResult);
    }
    #endregion
    //GetCategoryByName
    #region
    [Fact]
    public async Task GetCategoryByName_ValidParams_ExpectedResult()
    {
        var categoryDetail = ModelHelpers.Category.GenerateCategory().ToDetail();

        var categoryService = new Mock<ICategoryService>();
        categoryService.Setup(x => x.GetCategoryByNameAsync(categoryDetail.Id!, default)).ReturnsAsync(CommonHelpers.ApiResult.Ok(categoryDetail));

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.GetCategoryByName(categoryDetail.Id!, default);

        OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);

        var data = Assert.IsType<ApiDataResult<CategoryDetail>>(okObjectResult.Value);

        Assert.NotNull(data);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public async Task GetCategoryByName_InvalidParams_BadRequest(string name)
    {
        var categoryService = new Mock<ICategoryService>();

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.GetCategoryByName(name, default);

        Assert.True(result is BadRequestResult || result is BadRequestObjectResult);
    }

    [Fact]
    public async Task GetCategoryByName_ValidParams_NotFound()
    {
        var name = CommonHelpers.GenerateRandomString();

        var categoryService = new Mock<ICategoryService>();

        categoryService.Setup(x => x.GetCategoryByNameAsync(name, default)).ReturnsAsync((ApiDataResult<CategoryDetail>)null!);

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.GetCategoryByName(name, default);

        Assert.True(result is NotFoundResult || result is NotFoundObjectResult);
    }
    #endregion
    //CreateCategory
    #region
    [Fact]
    public async Task CreateCategory_ValidRequestBody_ExpectedResult()
    {
        var requestBody = ModelHelpers.Category.GenerateCreateRequestBody();
        var categoryDetail = requestBody.ToCreateCategory().ToDetail();

        var categoryService = new Mock<ICategoryService>();

        categoryService.Setup(x => x.CreateCategoryAsync(requestBody, default)).ReturnsAsync(CommonHelpers.ApiResult.Ok(categoryDetail));

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.CreateCategory(requestBody, default);
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);

        var data = Assert.IsType<ApiDataResult<CategoryDetail>>(okResult.Value);

        Assert.NotNull(data);
        Assert.NotNull(data.Data);
        Assert.Equal(data.Data.Name, requestBody.Name);
    }

    [Fact]
    public async Task CreateCategory_ValidRequestBody_NotFound()
    {
        var requestBody = ModelHelpers.Category.GenerateCreateRequestBody();
        var categoryDetail = requestBody.ToCreateCategory().ToDetail();

        var categoryService = new Mock<ICategoryService>();
        categoryService.Setup(x => x.CreateCategoryAsync(requestBody, default)).ReturnsAsync(CommonHelpers.ApiResult.NotFound(categoryDetail));

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.CreateCategory(requestBody, default);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Theory]
    [InlineData(null, null, "RequestBody is not allowed null.")]
    [InlineData("CategoryName", null, "Category Code is not allowed null.")]
    [InlineData(null, "CategoryCode", "Category Name is not allowed null.")]
    public async Task CreateCategory_InvalidRequestBody_BadRequest(string? name, string? categoryCode, string expectedErrorMessage)
    {
        var categoryService = new Mock<ICategoryService>();
        var controller = new CategoryController(categoryService.Object);

        var requestBody = new CreateCategoryRequestBody
        {
            Name = name,
            CategoryCode = categoryCode
        };

        var result = await controller.CreateCategory(requestBody, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.Equal(expectedErrorMessage, badRequestResult.Value);
    }
    #endregion
    //DeleteCategory
    #region
    [Fact]
    public async Task DeleteCategory_ValidParams_ExpectedResult()
    {
        var id = CommonHelpers.GenerateBsonId();

        var categoryService = new Mock<ICategoryService>();

        categoryService.Setup(x => x.DeleteCategoryAsync(id, default)).ReturnsAsync(new ApiStatusResult());

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.DeleteCategory(id, default);

        Assert.IsType<OkObjectResult>(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public async Task DeleteCategory_InvalidParams_BadRequest(string id)
    {
        var categoryService = new Mock<ICategoryService>();

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.DeleteCategory(id, default);

        Assert.True(result is BadRequestResult || result is BadRequestObjectResult);
    }

    [Fact]
    public async Task DeleteCategory_ValidParams_Problem()
    {
        var id = CommonHelpers.GenerateBsonId();

        var categoryService = new Mock<ICategoryService>();

        categoryService.Setup(x => x.DeleteCategoryAsync(id, default)).ReturnsAsync((ApiStatusResult)null!);

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.DeleteCategory(id, default);

        var objectResult = (ObjectResult)result;

        Assert.IsType<ProblemDetails>(objectResult.Value);

        var problemDetails = (ProblemDetails)objectResult.Value;

        Assert.Equal($"Cannot delete category with id: {id}", problemDetails.Detail);
    }
    #endregion
}
