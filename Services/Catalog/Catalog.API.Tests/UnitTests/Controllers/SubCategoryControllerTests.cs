﻿// using ApiClient.Catalog.SubCategory.Models;
// using ApiClient.Common;
// using Catalog.API.Controllers;
// using Catalog.API.Extensions;
// using Catalog.API.Services;
// using Catalog.API.Tests.Common;
// using Microsoft.AspNetCore.Mvc;
// using System.Net;
// using Microsoft.Extensions.Logging.Api;
// using UnitTest.Common.Helpers;
//
// namespace Catalog.API.Tests.UnitTests.Controllers;
//
// public class SubCategoryControllerTests
// {
//     [Fact]
//     public void Constructor_NullProductServiceParams_ThrowException()
//     {
//         ISubCategoryService subCategoryService = null!;
//         ICategoryService categoryService = null!;
//         ILogger<SubCategoryController> logger = null!;
//         
//         Assert.Throws<ArgumentNullException>(
//             nameof(subCategoryService),
//             () =>
//             {
//                 _ = new SubCategoryController(subCategoryService, categoryService, logger);
//             });
//     }
//     
//     #region GetSubCategory
//     [Fact]
//     public async Task GetSubCategory_ValidParams_NotFound()
//     {
//         //Config mock data -- tao data ảo
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var categoryService = new Mock<ICategoryService>();
//         var logger = new Mock<Logger<SubCategoryController>>();
//
//         subCategoryService.Setup(x => x.GetSubCategoriesAsync(default)).ReturnsAsync((List<SubCategorySummary>)null!);
//
//         //Run test
//         var controller = new SubCategoryController(subCategoryService.Object, categoryService.Object, logger.Object);
//
//         var result = await controller.GetSubCategories(default);
//
//         Assert.IsType<NotFoundResult>(result);
//
//     }
//
//     [Fact]
//     public async Task GetSubCategory_ValidParams_ExpectedResult()
//     {
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         var subCategorySummarise = ModelHelpers.SubCategory.GenerateSubCategorySummaries();
//
//         subCategoryService.Setup(x => x.GetSubCategoriesAsync(default)).ReturnsAsync(CommonHelpers.ApiResult.Ok(subCategorySummarise));
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.GetSubCategories(default);
//
//         OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
//
//         var data = Assert.IsType<ApiDataResult<List<SubCategorySummary>>>(okResult.Value);
//
//         Assert.NotNull(data.Data);
//         Assert.Equal(data.Data.Count, subCategorySummarise.Count);
//     }
//
//     #endregion
//     
//     #region GetSubCategoryById
//     [Theory]
//     [InlineData("")]
//     [InlineData("  ")]
//     public async Task GetSubCategoryById_InvalidParams_BadRequest(string id)
//     {
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.GetSubCategoryById(id, default);
//
//         Assert.IsType<BadRequestObjectResult>(result);
//     }
//
//     [Fact]
//     public async Task GetSubCategoryById_InvalidParams_NotFound()
//     {
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         var id = CommonHelpers.GenerateBsonId();
//         subCategoryService.Setup(x => x.GetSubCategoryByIdAsync(id, default)).ReturnsAsync((ApiDataResult<SubCategorySummary>)null!);
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.GetSubCategoryById(id, default);
//
//         Assert.IsType<NotFoundResult>(result);
//     }
//
//     [Fact]
//     public async Task GetSubCategoryById_ValidParams_ExpectedResult()
//     {
//         var subCategory = ModelHelpers.SubCategory.GenerateSubCategory().ToSummary();
//
//         var subCategoryService = new Mock<ISubCategoryService>();
//         subCategoryService.Setup(x => x.GetSubCategoryByIdAsync(subCategory.Id!, default)).ReturnsAsync(new ApiDataResult<SubCategorySummary>() { Data = subCategory });
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.GetSubCategoryById(subCategory.Id!, default);
//
//         OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);
//
//         var data = Assert.IsType<ApiDataResult<SubCategorySummary>>(okObjectResult.Value);
//
//         Assert.NotNull(data);
//     }
//     #endregion
//
//     #region GetSubCategoryByName
//     [Theory]
//     [InlineData("")]
//     [InlineData("  ")]
//     public async Task GetSubCategoryByName_InvalidParams_BadRequest(string name)
//     {
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.GetSubCategoryByName(name, default);
//
//         Assert.IsType<BadRequestObjectResult>(result);
//     }
//
//     [Fact]
//     public async Task GetSubCategoryByName_InvalidParams_NotFound()
//     {
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         var name = CommonHelpers.GenerateRandomString();
//         subCategoryService.Setup(x => x.GetSubCategoryByNameAsync(name, default)).ReturnsAsync((ApiDataResult<SubCategorySummary>)null!);
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.GetSubCategoryByName(name, default);
//
//         Assert.IsType<NotFoundResult>(result);
//     }
//
//     [Fact]
//     public async Task GetSubCategoryByName_ValidParams_ExpectedResult()
//     {
//         var subCategory = ModelHelpers.SubCategory.GenerateSubCategory().ToSummary();
//
//         var subCategoryService = new Mock<ISubCategoryService>();
//         subCategoryService.Setup(x => x.GetSubCategoryByNameAsync(subCategory.Name!, default)).ReturnsAsync(new ApiDataResult<SubCategorySummary>() { Data = subCategory });
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.GetSubCategoryByName(subCategory.Name!, default);
//
//         OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);
//
//         var data = Assert.IsType<ApiDataResult<SubCategorySummary>>(okObjectResult.Value);
//
//         Assert.NotNull(data);
//     }
//     #endregion
//
//     #region GetSubCategoryByCategoryId
//     [Theory]
//     [InlineData("")]
//     [InlineData(" ")]
//     public async Task GetSubCategoriesByCategoryId_InvalidParams_BadRequest(string categoryId)
//     {
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.GetSubCategoriesByCategoryId(categoryId, default);
//
//         Assert.IsType<BadRequestObjectResult>(result);
//     }
//
//     [Fact]
//     public async Task GetSubCategoriesByCategoryId_InvalidParams_NotFound()
//     {
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         var categoryId = CommonHelpers.GenerateRandomString();
//         subCategoryService.Setup(x => x.GetSubCategoriesByCategoryIdAsync(categoryId, default)).ReturnsAsync((ApiDataResult<List<SubCategorySummary>>)null!);
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.GetSubCategoriesByCategoryId(categoryId, default);
//
//         Assert.IsType<NotFoundResult>(result);
//     }
//     [Fact]
//     public async Task GetSubCategoriesByCategoryId_ValidParams_ExpectedResult()
//     {
//         var subCategory = ModelHelpers.SubCategory.GenerateSubCategory().ToSummary();
//
//         var subCategorySummarise = ModelHelpers.SubCategory.GenerateSubCategorySummaries();
//
//
//         var subCategoryService = new Mock<ISubCategoryService>();
//         subCategoryService.Setup(x => x.GetSubCategoriesByCategoryIdAsync(subCategory.CategoryId!, default)).ReturnsAsync(new ApiDataResult<List<SubCategorySummary>>() { Data = subCategorySummarise });
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.GetSubCategoriesByCategoryId(subCategory.CategoryId!, default);
//
//         OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);
//
//         var data = Assert.IsType<ApiDataResult<List<SubCategorySummary >>>(okObjectResult.Value);
//
//         Assert.NotNull(data);
//     }
//     #endregion
//
//     #region CreateSubCategory
//     [Fact]
//     public async Task CreateSubCategory_ValidParams_NotFound()
//     {
//         var requestBody = ModelHelpers.SubCategory.GenerateCreateRequestBody();
//         var subCategory = ModelHelpers.SubCategory.GenerateSubCategory().ToSummary();
//
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         subCategoryService.Setup(x => x.CreateSubCategoryAsync(requestBody, default)).ReturnsAsync(CommonHelpers.ApiResult.NotFound(subCategory));
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//         var result = await controller.CreateSubCategory(requestBody, default);
//
//         Assert.IsType<NotFoundObjectResult>(result);
//     }
//
//     [Fact]
//     public async Task CreateSubCategory_ValidParams_Problem()
//     {
//         var requestBody = ModelHelpers.SubCategory.GenerateCreateRequestBody();
//         var subCategory = ModelHelpers.SubCategory.GenerateSubCategory().ToSummary();
//
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         subCategoryService.Setup(x => x.CreateSubCategoryAsync(requestBody, default)).ReturnsAsync(CommonHelpers.ApiResult.Problem(subCategory));
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//         var result = await controller.CreateSubCategory(requestBody, default);
//
//         var objectResult = Assert.IsType<ObjectResult>(result);
//         Assert.Equal(objectResult.StatusCode, (int)HttpStatusCode.InternalServerError);
//
//     }
//
//     [Fact] 
//     public async Task CreateSubCategory_InvalidParams_BadRequest_RequestBodyNull()
//     {
//         CreateSubCategoryRequestBody requestBody = null!;
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.CreateSubCategory(requestBody, default);
//         Assert.IsType<BadRequestObjectResult>(result);
//     }
//
//     [Theory] 
//     [InlineData("")]
//     [InlineData(" ")]
//     public async Task CreateSubCategory_InvalidParams_BadRequest_NameNull(string name)
//     {
//         var requestBody = ModelHelpers.SubCategory.GenerateCreateRequestBody();
//         requestBody.Name = name;
//
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.CreateSubCategory(requestBody, default);
//         Assert.IsType<BadRequestObjectResult>(result);
//     }
//
//     [Theory]
//     [InlineData("")]
//     [InlineData(" ")]
//     public async Task CreateSubCategory_InvalidParams_BadRequest_SubCategoryCodeNull(string subCategoryCode)
//     {
//         var requestBody = ModelHelpers.SubCategory.GenerateCreateRequestBody();
//         requestBody.SubCategoryCode = subCategoryCode;
//
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.CreateSubCategory(requestBody, default);
//         Assert.IsType<BadRequestObjectResult>(result);
//     }
//
//     [Theory]
//     [InlineData("")]
//     [InlineData(" ")]
//     public async Task CreateSubCategory_NullCategoryIdCodeParams_BadRequest(string categoryId)
//     {
//         var requestBody = ModelHelpers.SubCategory.GenerateCreateRequestBody();
//         requestBody.CategoryId = categoryId;
//
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.CreateSubCategory(requestBody, default);
//         Assert.IsType<BadRequestObjectResult>(result);
//     }
//
//     [Fact]
//     public async Task CreatSubCategory_ValidParams_ExpectedResult()
//     {
//         var requestBody = ModelHelpers.SubCategory.GenerateCreateRequestBody();
//         var subCategoryEntity = ModelHelpers.SubCategory.GenerateSubCategoryEntity(); // Tạo biến chứa data, dưới dạng model của context
//         var subCategorySummary = subCategoryEntity.ToSummary();//Map từ Model của context qua model của Summary
//
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         subCategoryService.Setup(x => x.CreateSubCategoryAsync(requestBody, default)).ReturnsAsync(CommonHelpers.ApiResult.Ok(subCategorySummary));
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.CreateSubCategory(requestBody, default);
//
//         Assert.IsType<OkObjectResult>(result);
//     }
//     #endregion
//
//     #region UpdateSubCategory
//     [Fact]
//     public async Task UpdateSubCategory_InvalidNullRequestBodyParams_BadRequest()
//     {
//         UpdateSubCategoryRequestBody requestBody = null!;
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.UpdateSubCategory(requestBody, default);
//         Assert.IsType<BadRequestObjectResult>(result);
//     }
//
//     [Theory]
//     [InlineData("")]
//     [InlineData(" ")]
//     public async Task UpdateSubCategory_InvalidParams_BadRequest(string id)
//     {
//         var requestBody = ModelHelpers.SubCategory.GenerateUpdateRequestBody();
//         requestBody.Id = id;
//
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.UpdateSubCategory(requestBody, default);
//         Assert.IsType<BadRequestObjectResult>(result);
//     }
//
//     [Fact]
//     public async Task UpdateSubCategory_ValidParams_Problem()
//     {
//         var requestBody = ModelHelpers.SubCategory.GenerateUpdateRequestBody();
//         var subCategory = ModelHelpers.SubCategory.GenerateSubCategory().ToSummary();
//
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         subCategoryService.Setup(x => x.UpdateSubCategoryAsync(requestBody, default)).ReturnsAsync(CommonHelpers.ApiResult.Problem(subCategory));
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//         var result = await controller.UpdateSubCategory(requestBody, default);
//
//         var objectResult = Assert.IsType<ObjectResult>(result);
//         Assert.Equal(objectResult.StatusCode, (int)HttpStatusCode.InternalServerError);
//     }
//
//     [Fact]
//     public async Task UpdateSubCategory_ValidParams_NotFound()
//     {
//         string id = CommonHelpers.GenerateBsonId();
//         var requestBody = ModelHelpers.SubCategory.GenerateUpdateRequestBody(id);
//         var subCategory = ModelHelpers.SubCategory.GenerateSubCategory().ToSummary();
//
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         subCategoryService.Setup(x => x.UpdateSubCategoryAsync(requestBody, default)).ReturnsAsync(CommonHelpers.ApiResult.NotFound(subCategory));
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//         var result = await controller.UpdateSubCategory(requestBody, default);
//
//         var objectResult = Assert.IsType<NotFoundObjectResult>(result);
//         Assert.Equal(objectResult.StatusCode, (int)HttpStatusCode.NotFound);
//     }
//
//     [Fact]
//     public async Task UpdateSubCategory_ValidParams_ExpectefResult ()
//     {
//         string id = CommonHelpers.GenerateBsonId();
//         var requestBody = ModelHelpers.SubCategory.GenerateUpdateRequestBody(id);
//         var subCategoryEntity = ModelHelpers.SubCategory.GenerateSubCategoryEntity(id); // Tạo biến chứa data, dưới dạng model của context
//         var subCategorySummary = subCategoryEntity.ToSummary();
//
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         subCategoryService.Setup(x => x.UpdateSubCategoryAsync(requestBody, default)).ReturnsAsync(CommonHelpers.ApiResult.Ok(subCategorySummary));
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//         var result = await controller.UpdateSubCategory(requestBody, default);
//
//         OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);
//
//         var data = Assert.IsType<ApiDataResult<SubCategorySummary>>(okObjectResult.Value);
//         Assert.IsType<OkObjectResult>(result);
//         Assert.Equivalent(subCategorySummary, data.Data);
//     }
//
//     #endregion
//
//     #region DeleteSubCategory
//     [Theory]
//     [InlineData("")]
//     [InlineData(" ")]
//     public async Task DeleteSubCategory_InvalidParams_BadRequest(string id)
//     {
//        
//         var subCategoryService = new Mock<ISubCategoryService>();
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.DeleteSubCategory(id, default);
//         Assert.IsType<BadRequestObjectResult>(result);
//     }
//
//     [Fact]
//     public async Task DeleteSubCategory_ValidParams_ExpectedResult()
//     {
//         string id = CommonHelpers.GenerateBsonId();
//
//         var subCategoryService = new Mock<ISubCategoryService>();
//         subCategoryService.Setup(x => x.DeleteSubCategoryAsync(id, default)).ReturnsAsync(new ApiStatusResult());
//
//         var controller = new SubCategoryController(subCategoryService.Object);
//
//         var result = await controller.DeleteSubCategory(id, default);
//
//         Assert.IsType<OkObjectResult>(result);
//     }
//     #endregion
// }
