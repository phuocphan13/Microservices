// using System.Net;
// using ApiClient.Catalog.Product.Models;
// using Catalog.API.Controllers;
// using Catalog.API.Extensions;
// using Catalog.API.Services;
// using Microsoft.Extensions.Logging.Api;
// using Catalog.API.Tests.Common;
// using Microsoft.AspNetCore.Mvc;
// using Platform.ApiBuilder;
// using UnitTest.Common.Helpers;
//
// namespace Catalog.API.Tests.UnitTests.Controllers;
//
// public class ProductControllerTests
// {
//     [Fact]
//     public void Constructor_NullProductServiceParams_ThrowException()
//     {
//         IProductService productService = null!;
//         ICategoryService categoryService = null!;
//         ISubCategoryService subCategoryService = null!;
//         var logger = new Mock<ILogger<ProductController>>();
//
//         Assert.Throws<ArgumentNullException>(
//             nameof(productService),
//             () =>
//             {
//                 _ = new ProductController(productService, categoryService, subCategoryService, logger.Object);
//             });
//     }
//
//     #region GetProducts
//
//     [Fact]
//     public async Task GetProducts_ValidParams_NotFound()
//     {
//         //Config mock data
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>();
//         
//         productService.Setup(x => x.GetProductsAsync(default)).ReturnsAsync((List<ProductSummary>)null!);
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.GetProducts(default);
//
//         //Assert
//         Assert.IsType<NotFoundResult>(result);
//     }
//
//     [Fact]
//     public async Task GetProducts_ValidParams_ExpectedResult()
//     {
//         var productSummarise = ModelHelpers.Product.GenerateProductSummaries();
//         
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>();
//
//         productService.Setup(x => x.GetProductsAsync(default)).ReturnsAsync(productSummarise);
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.GetProducts(default);
//
//         //Assert
//         OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
//
//         var data = Assert.IsType<List<ProductSummary>>(okResult.Value);
//
//         Assert.NotNull(data);
//         Assert.Equal(data.Count, productSummarise.Count);
//     }
//
//     #endregion
//
//     #region GetProductById
//     [Theory]
//     [InlineData("")]
//     [InlineData("  ")]
//     public async Task GetProductById_InvalidParams_BadRequest(string id)
//     {
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>();
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.GetProductById(id, default);
//
//         //Assert
//         Assert.IsType<BadRequestObjectResult>(result);
//     }
//
//     [Fact]
//     public async Task GetProductById_ValidParams_NotFound()
//     {
//         //Config mock data
//         var id = CommonHelpers.GenerateBsonId();
//         
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>();
//         
//         productService.Setup(x => x.GetProductByIdAsync(string.Empty, default)).ReturnsAsync((ProductDetail)null!);
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.GetProductById(id, default);
//
//         //Assert
//         Assert.IsType<NotFoundResult>(result);
//     }
//
//     [Fact]
//     public async Task GetProductById_ValidParams_ExpectedResult()
//     {
//         //Config mock data
//         var id = CommonHelpers.GenerateBsonId();
//         var productDetail = ModelHelpers.Product.GenerateProductDetail(id);
//         
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>();
//         
//         productService.Setup(x => x.GetProductByIdAsync(It.IsAny<string>(), default)).ReturnsAsync(productDetail);
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.GetProductById(id, default);
//
//         //Assert
//         OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
//
//         var data = Assert.IsType<ProductDetail>(okResult.Value);
//
//         Assert.NotNull(data);
//         Assert.Equal(data.Id, id);
//     }
//
//     #endregion
//
//     #region GetProductByCategory
//     
//     [Theory]
//     [InlineData("")]
//     [InlineData("  ")]
//     public async Task GetProductByCategory_InvalidParams_BadRequest(string category)
//     {
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>(); 
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.GetProductByCategory(category, default);
//
//         //Assert
//         Assert.IsType<BadRequestObjectResult>(result);
//     }
//
//     [Fact]
//     public async Task GetProductByCategory_ValidParams_NotFound()
//     {
//         var category = CommonHelpers.GenerateRandomString();
//
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>(); 
//         
//         productService.Setup(x => x.GetProductsByCategoryAsync(string.Empty, default)).ReturnsAsync((List<ProductSummary>)null!);
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.GetProductByCategory(category, default);
//
//         //Assert
//         Assert.IsType<NotFoundResult>(result);
//     }
//
//     [Fact]
//     public async Task GetProductByCategory_ValidParams_ExpectedResult()
//     {
//         var category = CommonHelpers.GenerateRandomString();
//         var productSummarise = ModelHelpers.Product.GenerateProductSummaries();
//
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>(); 
//         
//         productService.Setup(x => x.GetProductsByCategoryAsync(category, default)).ReturnsAsync(productSummarise);
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.GetProductByCategory(category, default);
//
//         //Assert
//         OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
//
//         var data = Assert.IsType<List<ProductSummary>>(okResult.Value);
//
//         Assert.NotNull(data);
//         Assert.Equal(data.Count, productSummarise.Count);
//     }
//     #endregion
//
//     #region CreateProduct
//
//     [Fact]
//     public async Task CreateProduct_InvalidParams_BadRequest()
//     {
//         CreateProductRequestBody requestBody = null!;
//         
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>(); 
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.CreateProduct(requestBody, default);
//         Assert.IsType<BadRequestObjectResult>(result);
//     }
//
//     [Fact]
//     public async Task CreateProduct_ValidParams_NotFound()
//     {
//         var requestBody = ModelHelpers.Product.GenerateCreateRequestBody();
//
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>(); 
//         
//         productService.Setup(x => x.CheckExistingAsync(It.IsAny<string>(), It.IsAny<PropertyName>(), default)).ReturnsAsync(true);
//         // productService.Setup(x => x.CreateProductAsync(requestBody, default)).ReturnsAsync((ProductDetail)null!);
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.CreateProduct(requestBody, default);
//         Assert.IsType<NotFoundObjectResult>(result);
//     }
//
//     [Fact]
//     public async Task CreateProduct_ValidParams_Problem()
//     {
//         //Create Data for testing
//         var requestBody = ModelHelpers.Product.GenerateCreateRequestBody();
//
//         //Initialize Mock 
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>(); 
//         
//         //Setup Mock function
//         productService.Setup(x => x.CheckExistingAsync(It.IsAny<string>(), It.IsAny<PropertyName>(), default)).ReturnsAsync(false);
//         productService.Setup(x => x.CreateProductAsync(requestBody, default)).ReturnsAsync((ProductDetail)null!);
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.CreateProduct(requestBody, default);
//         
//         //Test
//         var objectResult = Assert.IsType<ObjectResult>(result);
//         Assert.Equal(objectResult.StatusCode, (int)HttpStatusCode.InternalServerError);
//     }
//
//     [Fact]
//     public async Task CreateProduct_ValidParams_ExpectedResult()
//     {
//         var requestBody = ModelHelpers.Product.GenerateCreateRequestBody();
//         var productDetail = requestBody.ToCreateProduct().ToDetail();
//
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>(); 
//         
//         productService.Setup(x => x.CheckExistingAsync(It.IsAny<string>(), It.IsAny<PropertyName>(), default)).ReturnsAsync(false);
//         productService.Setup(x => x.CreateProductAsync(requestBody, default)).ReturnsAsync(productDetail);
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.CreateProduct(requestBody, default);
//         OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
//
//         var data = Assert.IsType<ApiDataResult<ProductDetail>>(okResult.Value);
//
//         Assert.NotNull(data);
//         Assert.NotNull(data.Result);
//         Assert.Equal(data.Result.Name, requestBody.Name);
//     }
//
//     #endregion
//     
//     #region UpdateProduct
//
//     [Theory]
//     [InlineData(null!)]
//     public async Task UpdateProduct_InvalidParams_BadRequest(UpdateProductRequestBody? requestBody)
//     {
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>(); 
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.UpdateProduct(requestBody!, default);
//         Assert.IsType<BadRequestObjectResult>(result);
//     }
//
//     [Fact]
//     public async Task UpdateProduct_ValidParams_NotFound()
//     {
//         string id = CommonHelpers.GenerateBsonId();
//         var requestBody = ModelHelpers.Product.GenerateUpdateRequestBody(id);
//         
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>(); 
//         
//         productService.Setup(x => x.CheckExistingAsync(It.IsAny<string>(), It.IsAny<PropertyName>(), default)).ReturnsAsync(false);
//         productService.Setup(x => x.UpdateProductAsync(requestBody, default)).ReturnsAsync((ProductDetail)null!);
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.UpdateProduct(requestBody, default);
//         Assert.IsType<NotFoundObjectResult>(result);
//     }
//
//     [Fact]
//     public async Task UpdateProduct_ValidParams_Problem()
//     {
//         var requestBody = ModelHelpers.Product.GenerateUpdateRequestBody();
//
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>(); 
//         
//         productService.Setup(x => x.CheckExistingAsync(It.IsAny<string>(), It.IsAny<PropertyName>(), default)).ReturnsAsync(true);
//         productService.Setup(x => x.UpdateProductAsync(requestBody, default)).ReturnsAsync((ProductDetail)null!);
//         
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.UpdateProduct(requestBody, default);
//         var objectResult = Assert.IsType<ObjectResult>(result);
//         
//         Assert.Equal(objectResult.StatusCode, (int)HttpStatusCode.InternalServerError);
//     }
//
//     [Fact]
//     public async Task UpdateProduct_ValidParams_ExpectedResult()
//     {
//         string id = CommonHelpers.GenerateBsonId();
//         var requestBody = ModelHelpers.Product.GenerateUpdateRequestBody(id);
//         var entity = ModelHelpers.Product.GenerateProductEntity(id);
//         entity.ToUpdateProduct(requestBody);
//         var productDetail = entity.ToDetail();
//
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>(); 
//         
//         productService.Setup(x => x.CheckExistingAsync(It.IsAny<string>(), It.IsAny<PropertyName>(), default)).ReturnsAsync(true);
//         productService.Setup(x => x.UpdateProductAsync(requestBody, default)).ReturnsAsync(productDetail);
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.UpdateProduct(requestBody, default);
//         OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
//
//         var data = Assert.IsType<ApiDataResult<ProductDetail>>(okResult.Value);
//
//         Assert.NotNull(data);
//         Assert.NotNull(data.Result);
//         Assert.Equal(data.Result.Name, requestBody.Name);
//         Assert.Equal(data.Result.Id, id);
//     }
//
//     #endregion
//
//     #region DeleteProduct
//     
//     [Theory]
//     [InlineData("")]
//     [InlineData("  ")]
//     public async Task DeleteProduct_InvalidIdParams_BadRequest(string id)
//     {
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>(); 
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.DeleteProductById(id, default);
//         Assert.IsType<BadRequestObjectResult>(result);
//     }
//
//     [Fact]
//     public async Task DeleteProduct_ValidParams_ExpectedResult()
//     {
//         string id = CommonHelpers.GenerateBsonId();
//
//         var productService = new Mock<IProductService>();
//         var categoryService = new Mock<ICategoryService>();
//         var subCategoryService = new Mock<ISubCategoryService>();
//         var logger = new Mock<ILogger<ProductController>>(); 
//         
//         productService.Setup(x => x.DeleteProductAsync(id, default)).ReturnsAsync(true);
//
//         //Run testing
//         var controller = new ProductController(productService.Object, categoryService.Object, subCategoryService.Object, logger.Object);
//
//         var result = await controller.DeleteProductById(id, default);
//         OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
//
//         var data = Assert.IsType<bool>(okResult.Value);
//
//         Assert.True(data);
//     }
//
//     #endregion
// }