using ApiClient.Catalog.Category.Models;
using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Common;
using Catalog.API.Controllers;
using Catalog.API.Extensions;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
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
        categoryService.Setup(x => x.GetCategoriesAsync(default)).ReturnsAsync(new ApiDataResult<List<CategorySummary>>() { Data = categorySummarise });

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
        categoryService.Setup(x => x.GetCategoryByIdAsync(categoryDetail.Id!, default)).ReturnsAsync(new ApiDataResult<CategoryDetail>() { Data = categoryDetail });

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.GetCategoryById(categoryDetail.Id!, default);

        OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);

        var data = Assert.IsType<ApiDataResult<CategoryDetail>>(okObjectResult.Value);

        Assert.NotNull(data);
    }

    [Fact]
    public async Task GetCategoryById_ValidParams_BadRequest()
    {
        var categoryService = new Mock<ICategoryService>();

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.GetCategoryById(null!, default);

        Assert.True(result is BadRequestResult || result is BadRequestObjectResult);
    }

    [Fact]
    public async Task GetCategoryById_ValidParams_NotFound()
    {
        var categoryService = new Mock<ICategoryService>();

        categoryService.Setup(x => x.GetCategoryByIdAsync("id", default)).ReturnsAsync((ApiDataResult<CategoryDetail>)null!);

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.GetCategoryById("id", default);

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
        categoryService.Setup(x => x.GetCategoryByNameAsync(categoryDetail.Id!, default)).ReturnsAsync(new ApiDataResult<CategoryDetail>() { Data = categoryDetail });

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.GetCategoryByName(categoryDetail.Id!, default);

        OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);

        var data = Assert.IsType<ApiDataResult<CategoryDetail>>(okObjectResult.Value);

        Assert.NotNull(data);
    }

    [Fact]
    public async Task GetCategoryByName_InvalidParams_BadRequest()
    {
        var categoryService = new Mock<ICategoryService>();

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.GetCategoryByName(null!, default);

        Assert.True(result is BadRequestResult || result is BadRequestObjectResult);
    }

    [Fact]
    public async Task GetCategoryByName_ValidParams_NotFound()
    {
        var categoryService = new Mock<ICategoryService>();

        categoryService.Setup(x => x.GetCategoryByNameAsync("name", default)).ReturnsAsync((ApiDataResult<CategoryDetail>)null!);

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.GetCategoryByName("name", default);

        Assert.True(result is NotFoundResult || result is NotFoundObjectResult);
    }
    #endregion
    //DeleteCategory
    #region
    [Fact]
    public async Task DeleteCategory_ValidParams_ExpectedResult()
    {
        var categoryService = new Mock<ICategoryService>();

        categoryService.Setup(x => x.DeleteCategoryAsync("id", default)).ReturnsAsync(new ApiStatusResult());

        var controller = new CategoryController(categoryService.Object);

        var result = await controller.DeleteCategory("id", default);

        Assert.IsType<ApiStatusResult>(result);
        Assert.IsType<OkObjectResult>(result);
    }
    #endregion
}
