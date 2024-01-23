using ApiClient.Catalog.Category.Models;
using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Common;
using Catalog.API.Controllers;
using Catalog.API.Extensions;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UnitTest.Common.Helpers;
using ModelHelpers = Catalog.API.Tests.Common.ModelHelpers;

namespace Catalog.API.Tests.UnitTests.Controllers;

[Collection("CategoryControllerTests")]
public class CategoryControllerTests
{
    [Collection("CategoryControllerTests")]
    public class GetCategoriesTests 
    {
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
    }

    [Collection("CategoryControllerTests")]
    public class GetCategoryByIdTests
    {
        [Fact]
        public async Task GetCategoryById_ValidParams_ExpectedResult()
        {
            var categoryDetail = ModelHelpers.Category.GenerateCategory().ToDetail();

            var categoryService = new Mock<ICategoryService>();
            categoryService.Setup(x => x.GetCategoryByIdAsync(categoryDetail.Id!, default)).ReturnsAsync(CommonHelpers.ApiResult.Ok(categoryDetail));

            var controller = new CategoryController(categoryService.Object);

            var result = await controller.GetCategoryById(categoryDetail.Id!, default);

            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);

            var data = Assert.IsType<ApiDataResult<SubCategoryDetail>>(okObjectResult.Value);

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

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetCategoryById_ValidParams_NotFound()
        {
            var id = CommonHelpers.GenerateBsonId();

            var categoryService = new Mock<ICategoryService>();

            categoryService.Setup(x => x.GetCategoryByIdAsync(id, default)).ReturnsAsync((ApiDataResult<SubCategoryDetail>)null!);

            var controller = new CategoryController(categoryService.Object);

            var result = await controller.GetCategoryById(id, default);

            Assert.IsType<NotFoundResult>(result);
        }
    }

    [Collection("CategoryControllerTests")]
    public class GetCategoryByNameTests 
    {
        [Fact]
        public async Task GetCategoryByName_ValidParams_ExpectedResult()
        {
            var categoryDetail = ModelHelpers.Category.GenerateCategory().ToDetail();

            var categoryService = new Mock<ICategoryService>();
            categoryService.Setup(x => x.GetCategoryByNameAsync(categoryDetail.Name!, default)).ReturnsAsync(CommonHelpers.ApiResult.Ok(categoryDetail));

            var controller = new CategoryController(categoryService.Object);

            var result = await controller.GetCategoryByName(categoryDetail.Name!, default);

            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);

            var data = Assert.IsType<ApiDataResult<SubCategoryDetail>>(okObjectResult.Value);

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

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetCategoryByName_ValidParams_NotFound()
        {
            var name = CommonHelpers.GenerateRandomString();

            var categoryService = new Mock<ICategoryService>();

            categoryService.Setup(x => x.GetCategoryByNameAsync(name, default)).ReturnsAsync((ApiDataResult<SubCategoryDetail>)null!);

            var controller = new CategoryController(categoryService.Object);

            var result = await controller.GetCategoryByName(name, default);

            Assert.IsType<NotFoundResult>(result);
        } 
    }

    [Collection("CategoryControllerTests")]
    public class CreateCategoryTests
    {
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

            var data = Assert.IsType<ApiDataResult<SubCategoryDetail>>(okResult.Value);

            Assert.NotNull(data);
            Assert.NotNull(data.Data);
            Assert.Equal(data.Data.Name, requestBody.Name);
        }

        [Theory]
        [InlineData("CategoryName", "", "Category Code is not allowed null.")]
        [InlineData("CategoryName", " ", "Category Code is not allowed null.")]
        [InlineData("", "CategoryCode", "Category Name is not allowed null.")]
        [InlineData(" ", "CategoryCode", "Category Name is not allowed null.")]
        public async Task CreateCategory_InvalidRequestBody_BadRequest(string name, string categoryCode, string expectedErrorMessage)
        {
            var categoryService = new Mock<ICategoryService>();
            var controller = new CategoryController(categoryService.Object);

            var requestBody = new CreateCategoryRequestBody
            {
                Name = name,
                CategoryCode = categoryCode,
            };

            var result = await controller.CreateCategory(requestBody, default);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult);
            Assert.Equal(expectedErrorMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task CreateCategory_NullRequestBody_BadRequest()
        {
            CreateCategoryRequestBody requestBody = null!;
            var cateogryService = new Mock<ICategoryService>();

            var controller = new CategoryController(cateogryService.Object);

            var result = await controller.CreateCategory(requestBody, default);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateCategory_ValidParams_Problem()
        {
            var requestBody = ModelHelpers.Category.GenerateCreateRequestBody();
            var categoryDetail = requestBody.ToCreateCategory().ToDetail();

            var categoryService = new Mock<ICategoryService>();

            categoryService.Setup(x => x.CreateCategoryAsync(requestBody, default)).ReturnsAsync(CommonHelpers.ApiResult.Problem(categoryDetail));

            var controller = new CategoryController(categoryService.Object);

            var result = await controller.CreateCategory(requestBody, default);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(objectResult.StatusCode, (int)HttpStatusCode.InternalServerError);
        }
    }

    [Collection("CategoryControllerTests")]
    public class UpdateCategoryTests
    {
        [Theory]
        [InlineData("CategoryName", "", "Category Code is not allowed null.")]
        [InlineData("CategoryName", " ", "Category Code is not allowed null.")]
        [InlineData("", "CategoryCode", "Category Name is not allowed null.")]
        [InlineData(" ", "CategoryCode", "Category Name is not allowed null.")]
        public async Task UpdateCategory_InvalidRequestBody_BadRequest(string name, string categoryCode, string expectedErrorMessage)
        {
            var categoryService = new Mock<ICategoryService>();
            var controller = new CategoryController(categoryService.Object);

            var requestBody = new CreateCategoryRequestBody
            {
                Name = name,
                CategoryCode = categoryCode,
            };

            var result = await controller.CreateCategory(requestBody, default);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult);
            Assert.Equal(expectedErrorMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateCategory_NullRequestBody_BadRequest()
        {
            CreateCategoryRequestBody requestBody = null!;
            var cateogryService = new Mock<ICategoryService>();

            var controller = new CategoryController(cateogryService.Object);

            var result = await controller.CreateCategory(requestBody, default);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateCategory_ValidRequestBody_NotFound()
        {
            string id = CommonHelpers.GenerateBsonId();
            var requestBody = ModelHelpers.Category.GenerateUpdateRequestBody(id);
            var entity = ModelHelpers.Category.GenerateCategoryEntity(id);
            entity.ToUpdateCategory(requestBody);
            var categoryDetail = entity.ToDetail();

            var categoryService = new Mock<ICategoryService>();
            categoryService.Setup(x => x.UpdateCategoryAsync(requestBody, default)).ReturnsAsync(CommonHelpers.ApiResult.NotFound(categoryDetail));

            var controller = new CategoryController(categoryService.Object);

            var result = await controller.UpdateCategory(requestBody, default);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateCategory_ValidRequestBody_Problem()
        {
            var requestBody = ModelHelpers.Category.GenerateUpdateRequestBody();
            var entity = ModelHelpers.Category.GenerateCategoryEntity();
            entity.ToUpdateCategory(requestBody);
            var categoryDetail = entity.ToDetail();

            var categoryService = new Mock<ICategoryService>();
            categoryService.Setup(x => x.UpdateCategoryAsync(requestBody, default)).ReturnsAsync(CommonHelpers.ApiResult.Problem(categoryDetail));

            var controller = new CategoryController(categoryService.Object);

            var result = await controller.UpdateCategory(requestBody, default);
            var objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(objectResult.StatusCode, (int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task UpdateCategory_ValidRequestBody_ExpectedResult()
        {
            string id = CommonHelpers.GenerateBsonId();
            var requestBody = ModelHelpers.Category.GenerateUpdateRequestBody(id);
            var entity = ModelHelpers.Category.GenerateCategoryEntity(id);
            entity.ToUpdateCategory(requestBody);
            var categoryDetail = entity.ToDetail();

            var categoryService = new Mock<ICategoryService>();
            categoryService.Setup(x => x.UpdateCategoryAsync(requestBody, default)).ReturnsAsync(CommonHelpers.ApiResult.Ok(categoryDetail));

            var controller = new CategoryController(categoryService.Object);

            var result = await controller.UpdateCategory(requestBody, default);
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);

            var data = Assert.IsType<ApiDataResult<SubCategoryDetail>>(okResult.Value);

            Assert.NotNull(data);
            Assert.NotNull(data.Data);
            Assert.Equal(data.Data.Name, requestBody.Name);
            Assert.Equal(data.Data.Id, id);
        }
    }

    [Collection("CategoryControllerTests")]
    public class DeteleCategoryTests
    {
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

            Assert.IsType<BadRequestObjectResult>(result);
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
    }
}
