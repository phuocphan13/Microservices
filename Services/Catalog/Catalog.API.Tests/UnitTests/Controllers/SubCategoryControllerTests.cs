using ApiClient.Catalog.SubCategory.Models;
using ApiClient.Common;
using Catalog.API.Controllers;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Tests.UnitTests.Controllers;

public class SubCategoryControllerTests
{
    [Fact]
    public async Task GetSubCategory_ValidParams_NotFound()
    {
        //Config mock data -- tao data ảo
        var subCategoryService = new Mock<ISubCategoryService>();

        subCategoryService.Setup(x => x.GetSubCategoriesAsync(default)).ReturnsAsync((ApiDataResult<List<SubCategorySummary>>)null!);

        //Run test
        var controller = new SubCategoryController(subCategoryService.Object);

        var result = await controller.GetSubCategories(default);

        Assert.IsType<NotFoundResult>(result);


    }
}
