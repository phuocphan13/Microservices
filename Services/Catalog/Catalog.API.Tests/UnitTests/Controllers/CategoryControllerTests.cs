using ApiClient.Catalog.Category.Models;
using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Catalog.Product.Models;
using ApiClient.Common;
using Catalog.API.Controllers;
using Catalog.API.Extensions;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using ModelHelpers = Catalog.API.Tests.Common.ModelHelpers;

namespace Catalog.API.Tests.UnitTests.Controllers;

public class CategoryControllerTests
{
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
}
