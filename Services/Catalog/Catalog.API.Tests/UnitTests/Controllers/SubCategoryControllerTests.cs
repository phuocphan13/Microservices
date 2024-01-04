using ApiClient.Catalog.Category.Models;
using ApiClient.Catalog.Product.Models;
using ApiClient.Catalog.SubCategory.Models;
using ApiClient.Common;
using Catalog.API.Controllers;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Services;
using Catalog.API.Tests.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using UnitTest.Common.Helpers;

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

    [Fact]
    public async Task GetSubCategory_ValidParams_ExpectedResult()
    {
        var subCategoryService = new Mock<ISubCategoryService>();

        var subCategorySummarise = ModelHelpers.SubCategory.GenerateSubCategorySummaries();

        subCategoryService.Setup(x => x.GetSubCategoriesAsync(default)).ReturnsAsync(CommonHelpers.ApiResult.Ok(subCategorySummarise));
        //subCategoryService.Setup(x => x.GetSubCategoriesAsync(default)).ReturnsAsync(new ApiDataResult<List<SubCategorySummary>>() { Data = subCategorySummarise});

        var controller = new SubCategoryController(subCategoryService.Object);

        var result = await controller.GetSubCategories(default);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);

        var data = Assert.IsType<ApiDataResult<List<SubCategorySummary>>>(okResult.Value);

        Assert.NotNull(data.Data);
        Assert.Equal(data.Data.Count, subCategorySummarise.Count);

    }
    //GetSubCategoryById
    #region
    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public async Task GetSubCategoryById_InvalidParams_BadRequest(string id)
    {
        var subCategoryService = new Mock<ISubCategoryService>();

        var controller = new SubCategoryController(subCategoryService.Object);

        var result = await controller.GetSubCategoryById(id, default);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetSubCategoryById_InvalidParams_NotFound()
    {
        var subCategoryService = new Mock<ISubCategoryService>();

        var id = CommonHelpers.GenerateBsonId();
        subCategoryService.Setup(x => x.GetSubCategoryByIdAsync(id, default)).ReturnsAsync((ApiDataResult<SubCategorySummary>)null!);

        var controller = new SubCategoryController(subCategoryService.Object);

        var result = await controller.GetSubCategoryById(id, default);

        Assert.IsType<NotFoundResult>(result); // WHY? tai sao o tren truyen vao Object ma ko tra ra type NotFoundOjectResult?
    }

    [Fact]
    public async Task GetSubCategoryById_ValidParams_ExpectedResult()
    {
        var subCategory = ModelHelpers.SubCategory.GenerateSubCategory().ToSummary();

        var subCategoryService = new Mock<ISubCategoryService>();
        subCategoryService.Setup(x => x.GetSubCategoryByIdAsync(subCategory.Id!, default)).ReturnsAsync(new ApiDataResult<SubCategorySummary>() { Data = subCategory });

        var controller = new SubCategoryController(subCategoryService.Object);

        var result = await controller.GetSubCategoryById(subCategory.Id!, default);

        OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);

        var data = Assert.IsType<ApiDataResult<SubCategorySummary>>(okObjectResult.Value);

        Assert.NotNull(data);
    }
    #endregion

    //GetSubCategoryByName
    #region
    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public async Task GetSubCategoryByName_InvalidParams_BadRequest(string name)
    {
        var subCategoryService = new Mock<ISubCategoryService>();

        var controller = new SubCategoryController(subCategoryService.Object);

        var result = await controller.GetSubCategoryByName(name, default);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetSubCategoryByName_InvalidParams_NotFound()
    {
        var subCategoryService = new Mock<ISubCategoryService>();

        var name = CommonHelpers.GenerateRandomString();
        subCategoryService.Setup(x => x.GetSubCategoryByNameAsync(name, default)).ReturnsAsync((ApiDataResult<SubCategorySummary>)null!);

        var controller = new SubCategoryController(subCategoryService.Object);

        var result = await controller.GetSubCategoryByName(name, default);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetSubCategoryByName_ValidParams_ExpectedResult()
    {
        var subCategory = ModelHelpers.SubCategory.GenerateSubCategory().ToSummary();

        var subCategoryService = new Mock<ISubCategoryService>();
        subCategoryService.Setup(x => x.GetSubCategoryByNameAsync(subCategory.Name!, default)).ReturnsAsync(new ApiDataResult<SubCategorySummary>() { Data = subCategory });

        var controller = new SubCategoryController(subCategoryService.Object);

        var result = await controller.GetSubCategoryByName(subCategory.Name!, default);

        OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);

        var data = Assert.IsType<ApiDataResult<SubCategorySummary>>(okObjectResult.Value);

        Assert.NotNull(data);
    }
    #endregion

    //GetSubCategoryByCategoryId
    #region
    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public async Task GetSubCategoriesByCategoryId_InvalidParams_BadRequest(string categoryId)
    {
        var subCategoryService = new Mock<ISubCategoryService>();

        var controller = new SubCategoryController(subCategoryService.Object);

        var result = await controller.GetSubCategoriesByCategoryId(categoryId, default);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetSubCategoriesByCategoryId_InvalidParams_NotFound()
    {
        var subCategoryService = new Mock<ISubCategoryService>();

        var categoryId = CommonHelpers.GenerateRandomString();
        subCategoryService.Setup(x => x.GetSubCategoriesByCategoryIdAsync(categoryId, default)).ReturnsAsync((ApiDataResult<List<SubCategorySummary>>)null!);

        var controller = new SubCategoryController(subCategoryService.Object);

        var result = await controller.GetSubCategoriesByCategoryId(categoryId, default);

        Assert.IsType<NotFoundResult>(result);
    }
    #endregion

    //CreateSubCategory
    #region
    [Fact]
    public async Task CreateSubCategory_ValidParams_NotFound()
    {
        var requestBody = ModelHelpers.SubCategory.GenerateCreateRequestBody();
        var subCategory = ModelHelpers.SubCategory.GenerateSubCategory().ToSummary();

        var subCategoryService = new Mock<ISubCategoryService>();

        subCategoryService.Setup(x => x.CreateSubCategoryAsync(requestBody, default)).ReturnsAsync(CommonHelpers.ApiResult.NotFound(subCategory));

        var controller = new SubCategoryController(subCategoryService.Object);
        var result = await controller.CreateSubCategory(requestBody, default);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task CreateSubCategory_ValidParams_Problem()
    {
        var requestBody = ModelHelpers.SubCategory.GenerateCreateRequestBody();
        var subCategory = ModelHelpers.SubCategory.GenerateSubCategory().ToSummary();

        var subCategoryService = new Mock<ISubCategoryService>();

        subCategoryService.Setup(x => x.CreateSubCategoryAsync(requestBody, default)).ReturnsAsync(CommonHelpers.ApiResult.Problem(subCategory));

        var controller = new SubCategoryController(subCategoryService.Object);
        var result = await controller.CreateSubCategory(requestBody, default);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(objectResult.StatusCode, (int)HttpStatusCode.InternalServerError);

    }

    [Theory]
    [InlineData("Name", "", "")]
    [InlineData("Name", " ", "")]
    [InlineData("", "", "")]
    [InlineData(" ", "", "")]
    public async Task CreateSubCategory_InvalidParams_BadRequest(string name, string SubCategoryCode, string CategoryId)
    {
        CreateSubCategoryRequestBody requestBody = null!;
        var subCategoryService = new Mock<ISubCategoryService>();

        var controller = new SubCategoryController(subCategoryService.Object);

        var result = await controller.CreateSubCategory(requestBody, default);
        Assert.IsType<BadRequestObjectResult>(result);
    }
    #endregion
}
