// using System.Linq.Expressions;
// using ApiClient.Discount.Models.Discount;
// using Catalog.API.Entities;
// using Catalog.API.Extensions;
// using Catalog.API.Repositories;
// using Catalog.API.Services;
// using Catalog.API.Services.Grpc;
// using Catalog.API.Tests.Common;
// using UnitTest.Common.Helpers;
//
// namespace Catalog.API.Tests.UnitTests.Services;
//
// public class ProductServiceTests
// {
//     [Fact]
//     public void Constructor_NullProductRepositoryParams_ThrowException()
//     {
//         IRepository<Product> productRepository = null!;
//         var categoryRepository = new Mock<IRepository<Category>>();
//         var subCategoryRepository = new Mock<IRepository<SubCategory>>();
//         var discountGrpcService = new Mock<IDiscountGrpcService>();
//
//         Assert.Throws<ArgumentNullException>(
//             nameof(productRepository),
//             () =>
//             {
//                 _ = new ProductService(productRepository, categoryRepository.Object, subCategoryRepository.Object, discountGrpcService.Object);
//             });
//     }
//
//     [Fact]
//     public void Constructor_NullCategoryRepositoryParams_ThrowException()
//     {
//         var productRepository = new Mock<IRepository<Product>>();
//         IRepository<Category> categoryRepository = null!;
//         var subCategoryRepository = new Mock<IRepository<SubCategory>>();
//         var discountGrpcService = new Mock<IDiscountGrpcService>();
//
//         Assert.Throws<ArgumentNullException>(
//             nameof(categoryRepository),
//             () =>
//             {
//                 _ = new ProductService(productRepository.Object, categoryRepository, subCategoryRepository.Object, discountGrpcService.Object);
//             });
//     }
//
//     [Fact]
//     public void Constructor_NullSubCategoryRepositoryParams_ThrowException()
//     {
//         var productRepository = new Mock<IRepository<Product>>();
//         var categoryRepository = new Mock<IRepository<Category>>();
//         IRepository<SubCategory> subCategoryRepository = null!;
//         var discountGrpcService = new Mock<IDiscountGrpcService>();
//
//         Assert.Throws<ArgumentNullException>(
//             nameof(subCategoryRepository),
//             () =>
//             {
//                 _ = new ProductService(productRepository.Object, categoryRepository.Object, subCategoryRepository, discountGrpcService.Object);
//             });
//     }
//
//     [Fact]
//     public void Constructor_NullDiscountGrpcServiceParams_ThrowException()
//     {
//         var productRepository = new Mock<IRepository<Product>>();
//         var categoryRepository = new Mock<IRepository<Category>>();
//         var subCategoryRepository = new Mock<IRepository<SubCategory>>();
//         IDiscountGrpcService discountGrpcService = null!;
//
//         Assert.Throws<ArgumentNullException>(
//             nameof(discountGrpcService),
//             () =>
//             {
//                 _ = new ProductService(productRepository.Object, categoryRepository.Object, subCategoryRepository.Object, discountGrpcService);
//             });
//     }
//
//     #region GetProductsAsync
//     [Fact]
//     public async Task GetProductsAsync_ValidParams_NotFoundSubData()
//     {
//         var categories = ModelHelpers.Category.GenerateCategories();
//         var subCategories = ModelHelpers.SubCategory.GenerateSubCategories();
//         var entities = ModelHelpers.Product.GenerateProductEntities();
//
//         var productRepository = new Mock<IRepository<Product>>();
//         var categoryRepository = new Mock<IRepository<Category>>();
//         var subCategoryRepository = new Mock<IRepository<SubCategory>>();
//         var discountGrpcService = new Mock<IDiscountGrpcService>();
//
//         productRepository.Setup(x => x.GetEntitiesAsync(default)).ReturnsAsync(entities);
//         categoryRepository.Setup(x => x.GetEntitiesQueryAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(categories);
//         subCategoryRepository.Setup(x => x.GetEntitiesQueryAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(subCategories);
//
//         IProductService service = new ProductService(productRepository.Object, categoryRepository.Object, subCategoryRepository.Object, discountGrpcService.Object);
//
//         var products = await service.GetProductsAsync();
//
//         Assert.NotNull(products);
//         Assert.Equal(entities.Count, products.Count);
//         Assert.True(products.All(x => string.IsNullOrWhiteSpace(x.Category)));
//         Assert.True(products.All(x => string.IsNullOrWhiteSpace(x.SubCategory)));
//     }
//
//     [Fact]
//     public async Task GetProductsAsync_ValidParams_ExpectedResult()
//     {
//         var categories = ModelHelpers.Category.GenerateCategories();
//         var subCategories = new List<SubCategory>()
//         {
//             ModelHelpers.SubCategory.GenerateSubCategory(categoryId: categories[0].Id),
//             ModelHelpers.SubCategory.GenerateSubCategory(categoryId: categories[1].Id)
//         };
//
//         var productEntities = new List<Product>()
//         {
//             ModelHelpers.Product.GenerateProductEntity(string.Empty, categories[0].Id, subCategories[0].Id),
//             ModelHelpers.Product.GenerateProductEntity(string.Empty, categories[1].Id, subCategories[1].Id),
//         };
//
//         var productRepository = new Mock<IRepository<Product>>();
//         var categoryRepository = new Mock<IRepository<Category>>();
//         var subCategoryRepository = new Mock<IRepository<SubCategory>>();
//         var discountGrpcService = new Mock<IDiscountGrpcService>();
//
//         productRepository.Setup(x => x.GetEntitiesAsync(default)).ReturnsAsync(productEntities);
//         categoryRepository.Setup(x => x.GetEntitiesQueryAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(categories);
//         subCategoryRepository.Setup(x => x.GetEntitiesQueryAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(subCategories);
//
//         IProductService service = new ProductService(productRepository.Object, categoryRepository.Object, subCategoryRepository.Object, discountGrpcService.Object);
//
//         var products = await service.GetProductsAsync();
//
//         Assert.NotNull(products);
//         Assert.Equal(productEntities.Count, products.Count);
//         Assert.Contains(products, x => categories.Any(i => i.Name == x.Category));
//         Assert.Contains(products, x => subCategories.Any(i => i.Name == x.SubCategory));
//     }
//
//     [Fact]
//     public async Task GetProductsAsync_ValidParams_WithDiscount_ExpectedResult()
//     {
//         decimal price = 100;
//         var categories = ModelHelpers.Category.GenerateCategories();
//         var subCategories = new List<SubCategory>()
//         {
//             ModelHelpers.SubCategory.GenerateSubCategory(categoryId: categories[0].Id),
//             ModelHelpers.SubCategory.GenerateSubCategory(categoryId: categories[1].Id)
//         };
//
//         var productEntities = new List<Product>()
//         {
//             ModelHelpers.Product.GenerateProductEntity(string.Empty, categories[0].Id, subCategories[0].Id, initAction: x =>
//             {
//                 x.Price = price;
//             }),
//             ModelHelpers.Product.GenerateProductEntity(string.Empty, categories[1].Id, subCategories[1].Id),
//         };
//
//         var response = new List<DiscountDetail>()
//         {
//             new() { CatalogCode = productEntities[0].ProductCode, Type  = DiscountEnum.Product, Amount = 10}
//         };
//
//         var productRepository = new Mock<IRepository<Product>>();
//         var categoryRepository = new Mock<IRepository<Category>>();
//         var subCategoryRepository = new Mock<IRepository<SubCategory>>();
//         var discountGrpcService = new Mock<IDiscountGrpcService>();
//
//         productRepository.Setup(x => x.GetEntitiesAsync(default)).ReturnsAsync(productEntities);
//         categoryRepository.Setup(x => x.GetEntitiesQueryAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(categories);
//         subCategoryRepository.Setup(x => x.GetEntitiesQueryAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(subCategories);
//         discountGrpcService.Setup(x => x.GetListDiscountsByCatalogCodeAsync(It.IsAny<DiscountEnum>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(response);
//
//         IProductService service = new ProductService(productRepository.Object, categoryRepository.Object, subCategoryRepository.Object, discountGrpcService.Object);
//
//         var products = await service.GetProductsAsync();
//
//         Assert.NotNull(products);
//         Assert.Equal(productEntities.Count, products.Count);
//         Assert.Contains(products, x => categories.Any(i => i.Name == x.Category));
//         Assert.Contains(products, x => subCategories.Any(i => i.Name == x.SubCategory));
//         Assert.Equal(price - response[0].Amount, products[0].Price);
//     }
//     #endregion
//
//     #region CreateProductAsync
//
//     [Fact]
//     public async Task CreateProductAsync_ValidParams_SaveFailure()
//     {
//         var category = ModelHelpers.Category.GenerateCategory();
//         var subCategory = ModelHelpers.SubCategory.GenerateSubCategory(categoryId: category.Id);
//         var requestBody = ModelHelpers.Product.GenerateCreateRequestBody(category.Name!, subCategory.Name!);
//
//         var entity = requestBody.ToCreateProduct();
//         entity.CategoryId = category.Id;
//         entity.SubCategoryId = subCategory.Id;
//
//         var productRepository = new Mock<IRepository<Product>>();
//         var categoryRepository = new Mock<IRepository<Category>>();
//         var subCategoryRepository = new Mock<IRepository<SubCategory>>();
//         var discountGrpcService = new Mock<IDiscountGrpcService>();
//
//         productRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>(), default)).ReturnsAsync(false);
//         productRepository.Setup(x => x.CreateEntityAsync(It.IsAny<Product>(), default)).ReturnsAsync(entity);
//         categoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(category);
//         subCategoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(subCategory);
//
//         IProductService service = new ProductService(productRepository.Object, categoryRepository.Object, subCategoryRepository.Object, discountGrpcService.Object);
//
//         var result = await service.CreateProductAsync(requestBody);
//
//         Assert.Null(result);
//     }
//
//     [Fact]
//     public async Task CreateProductAsync_ValidParams_ExpectedResult()
//     {
//         var category = ModelHelpers.Category.GenerateCategory();
//         var subCategory = ModelHelpers.SubCategory.GenerateSubCategory(categoryId: category.Id);
//         var requestBody = ModelHelpers.Product.GenerateCreateRequestBody(category.Name!, subCategory.Name!);
//
//         var entity = requestBody.ToCreateProduct();
//         entity.Id = CommonHelpers.GenerateBsonId();
//         entity.CategoryId = category.Id;
//         entity.SubCategoryId = subCategory.Id;
//
//         var productRepository = new Mock<IRepository<Product>>();
//         var categoryRepository = new Mock<IRepository<Category>>();
//         var subCategoryRepository = new Mock<IRepository<SubCategory>>();
//         var discountGrpcService = new Mock<IDiscountGrpcService>();
//
//         productRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>(), default)).ReturnsAsync(false);
//         productRepository.Setup(x => x.CreateEntityAsync(It.IsAny<Product>(), default)).ReturnsAsync(entity);
//         categoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(category);
//         subCategoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(subCategory);
//
//         IProductService service = new ProductService(productRepository.Object, categoryRepository.Object, subCategoryRepository.Object, discountGrpcService.Object);
//
//         var result = await service.CreateProductAsync(requestBody);
//
//         Assert.NotNull(result);
//
//         Assert.False(string.IsNullOrWhiteSpace(result.Id));
//         Assert.Equal(result.Category, category.Name);
//         Assert.Equal(result.SubCategory, subCategory.Name);
//     }
//     #endregion
//
//     #region UpdateProductAsync
//     [Fact]
//     public async Task UpdateProductAsync_ValidParams_SaveFailure()
//     {
//         var category = ModelHelpers.Category.GenerateCategory();
//         var subCategory = ModelHelpers.SubCategory.GenerateSubCategory(categoryId: category.Id);
//         var requestBody = ModelHelpers.Product.GenerateUpdateRequestBody(category.Name!, subCategory.Name!);
//
//         var entity = ModelHelpers.Product.GenerateProductEntity(initAction: x =>
//         {
//             x.Id = requestBody.Id!;
//         });
//
//         var productRepository = new Mock<IRepository<Product>>();
//         var categoryRepository = new Mock<IRepository<Category>>();
//         var subCategoryRepository = new Mock<IRepository<SubCategory>>();
//         var discountGrpcService = new Mock<IDiscountGrpcService>();
//
//         productRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>(), default)).ReturnsAsync(entity);
//         productRepository.Setup(x => x.UpdateEntityAsync(It.IsAny<Product>(), default)).ReturnsAsync(false);
//         categoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(category);
//         subCategoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(subCategory);
//
//         IProductService service = new ProductService(productRepository.Object, categoryRepository.Object, subCategoryRepository.Object, discountGrpcService.Object);
//
//         var result = await service.UpdateProductAsync(requestBody);
//
//         Assert.Null(result);
//     }
//
//     [Fact]
//     public async Task UpdateProductAsync_ValidParams_ExpectedResult()
//     {
//         var categoryName = CommonHelpers.GenerateRandomString();
//         
//         var category = ModelHelpers.Category.GenerateCategory();
//         var subCategory = ModelHelpers.SubCategory.GenerateSubCategory(categoryId: category.Id, initAction: x =>
//         {
//             x.CategoryId = categoryName;
//         });
//         
//         var requestBody = ModelHelpers.Product.GenerateUpdateRequestBody();
//
//         var entity = ModelHelpers.Product.GenerateProductEntity(initAction: x =>
//         {
//             x.Id = requestBody.Id!;
//         });
//
//         var productRepository = new Mock<IRepository<Product>>();
//         var categoryRepository = new Mock<IRepository<Category>>();
//         var subCategoryRepository = new Mock<IRepository<SubCategory>>();
//         var discountGrpcService = new Mock<IDiscountGrpcService>();
//
//         productRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>(), default)).ReturnsAsync(entity);
//         productRepository.Setup(x => x.UpdateEntityAsync(It.IsAny<Product>(), default)).ReturnsAsync(true);
//         categoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(category);
//         subCategoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(subCategory);
//
//         IProductService service = new ProductService(productRepository.Object, categoryRepository.Object, subCategoryRepository.Object, discountGrpcService.Object);
//
//         var result = await service.UpdateProductAsync(requestBody);
//
//         Assert.NotNull(result);
//
//         Assert.False(string.IsNullOrWhiteSpace(result.Id));
//         Assert.Equal(result.Category, category.Name);
//         Assert.Equal(result.SubCategory, subCategory.Name);
//     }
//     #endregion
//
//     #region DeleteProductAsync
//     [Fact]
//     public async Task DeleteProductAsync_ValidParams_NotFound()
//     {
//         string id = CommonHelpers.GenerateBsonId();
//
//         var productRepository = new Mock<IRepository<Product>>();
//         var categoryRepository = new Mock<IRepository<Category>>();
//         var subCategoryRepository = new Mock<IRepository<SubCategory>>();
//         var discountGrpcService = new Mock<IDiscountGrpcService>();
//
//         productRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>(), default)).ReturnsAsync((Product)null!);
//         productRepository.Setup(x => x.DeleteEntityAsync(It.IsAny<string>(), default)).ReturnsAsync(true);
//
//         IProductService service = new ProductService(productRepository.Object, categoryRepository.Object, subCategoryRepository.Object, discountGrpcService.Object);
//
//         var result = await service.DeleteProductAsync(id);
//
//         Assert.False(result);
//     }
//
//     [Fact]
//     public async Task DeleteProductAsync_ValidParams_DeleteFailure()
//     {
//         string id = CommonHelpers.GenerateBsonId();
//         var entity = ModelHelpers.Product.GenerateProductEntity();
//
//         var productRepository = new Mock<IRepository<Product>>();
//         var categoryRepository = new Mock<IRepository<Category>>();
//         var subCategoryRepository = new Mock<IRepository<SubCategory>>();
//         var discountGrpcService = new Mock<IDiscountGrpcService>();
//
//         productRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>(), default)).ReturnsAsync(entity);
//         productRepository.Setup(x => x.DeleteEntityAsync(It.IsAny<string>(), default)).ReturnsAsync(false);
//
//         IProductService service = new ProductService(productRepository.Object, categoryRepository.Object, subCategoryRepository.Object, discountGrpcService.Object);
//
//         var result = await service.DeleteProductAsync(id);
//
//         Assert.False(result);
//     }
//
//     [Fact]
//     public async Task DeleteProductAsync_ValidParams_ExpectedResult()
//     {
//         string id = CommonHelpers.GenerateBsonId();
//         var entity = ModelHelpers.Product.GenerateProductEntity();
//
//         var productRepository = new Mock<IRepository<Product>>();
//         var categoryRepository = new Mock<IRepository<Category>>();
//         var subCategoryRepository = new Mock<IRepository<SubCategory>>();
//         var discountGrpcService = new Mock<IDiscountGrpcService>();
//
//         productRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>(), default)).ReturnsAsync(entity);
//         productRepository.Setup(x => x.DeleteEntityAsync(It.IsAny<string>(), default)).ReturnsAsync(true);
//
//         IProductService service = new ProductService(productRepository.Object, categoryRepository.Object, subCategoryRepository.Object, discountGrpcService.Object);
//
//         var result = await service.DeleteProductAsync(id);
//
//         Assert.True(result);
//     }
//     #endregion
// }