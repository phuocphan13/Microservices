using System.Linq.Expressions;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Repositories;
using Catalog.API.Services;
using Catalog.API.Tests.Common;
using UnitTest.Common.Helpers;

namespace Catalog.API.Tests.UnitTests.Services;

public class ProductServiceTests
{
    [Fact]
    public void Constructor_NullProductRepositoryParams_ThrowException()
    {
        IRepository<Product> productRepository = null!;
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryRepository = new Mock<IRepository<SubCategory>>();

        Assert.Throws<ArgumentNullException>(
            nameof(productRepository),
            () =>
            {
                _ = new ProductService(productRepository, categoryRepository.Object, subCategoryRepository.Object);
            });
    }

    [Fact]
    public void Constructor_NullCategoryRepositoryParams_ThrowException()
    {
        var productRepository = new Mock<IRepository<Product>>();
        IRepository<Category> categoryRepository = null!;
        var subCategoryRepository = new Mock<IRepository<SubCategory>>();

        Assert.Throws<ArgumentNullException>(
            nameof(categoryRepository),
            () =>
            {
                _ = new ProductService(productRepository.Object, categoryRepository, subCategoryRepository.Object);
            });
    }

    [Fact]
    public void Constructor_NullSubCategoryRepositoryParams_ThrowException()
    {
        var productRepository = new Mock<IRepository<Product>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        IRepository<SubCategory> subCategoryRepository = null!;

        Assert.Throws<ArgumentNullException>(
            nameof(subCategoryRepository),
            () =>
            {
                _ = new ProductService(productRepository.Object, categoryRepository.Object, subCategoryRepository);
            });
    }

    #region GetProductsAsync

    [Fact]
    public async Task GetProductsAsync_ValidParams_NotFoundSubData()
    {
        var categories = ModelHelpers.Category.GenerateCategories();
        var subCategories = ModelHelpers.SubCategory.GenerateSubCategories();
        var entities = ModelHelpers.Product.GenerateProductEntities();

        var productRepository = new Mock<IRepository<Product>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryRepository = new Mock<IRepository<SubCategory>>();

        productRepository.Setup(x => x.GetEntitiesAsync(default)).ReturnsAsync(entities);
        categoryRepository.Setup(x => x.GetEntitiesQueryAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(categories);
        subCategoryRepository.Setup(x => x.GetEntitiesQueryAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(subCategories);

        IProductService service = new ProductService(productRepository.Object, categoryRepository.Object, subCategoryRepository.Object);

        var products = await service.GetProductsAsync();

        Assert.NotNull(products);
        Assert.Equal(entities.Count, products.Count);
        Assert.True(products.All(x => string.IsNullOrWhiteSpace(x.Category)));
        Assert.True(products.All(x => string.IsNullOrWhiteSpace(x.SubCategory)));
    }

    [Fact]
    public async Task GetProductsAsync_ValidParams_ExpectedResult()
    {
        //2 Cate, 2 SubCate, 2 Products
        //1 Cate - 1 SubCate - 1 Product
        var categories = ModelHelpers.Category.GenerateCategories();
        var subCategories = new List<SubCategory>()
        {
            ModelHelpers.SubCategory.GenerateSubCategory(categoryId: categories[0].Id),
            ModelHelpers.SubCategory.GenerateSubCategory(categoryId: categories[1].Id)
        };

        var productEntities = new List<Product>()
        {
            ModelHelpers.Product.GenerateProductEntity(string.Empty, categories[0].Id, subCategories[0].Id),
            ModelHelpers.Product.GenerateProductEntity(string.Empty, categories[1].Id, subCategories[1].Id),
        };

        var productRepository = new Mock<IRepository<Product>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryRepository = new Mock<IRepository<SubCategory>>();

        productRepository.Setup(x => x.GetEntitiesAsync(default)).ReturnsAsync(productEntities);
        categoryRepository.Setup(x => x.GetEntitiesQueryAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(categories);
        subCategoryRepository.Setup(x => x.GetEntitiesQueryAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(subCategories);

        IProductService service = new ProductService(productRepository.Object, categoryRepository.Object, subCategoryRepository.Object);

        var products = await service.GetProductsAsync();

        Assert.NotNull(products);
        Assert.Equal(productEntities.Count, products.Count);
        Assert.Contains(products, x => categories.Any(i => i.Name == x.Category));
        Assert.Contains(products, x => subCategories.Any(i => i.Name == x.SubCategory));
    }

    #endregion

    #region CreateProductAsync

    [Fact]
    public async Task CreateProductAsync_ValidParams_ExpectedResult()
    {
        var category = ModelHelpers.Category.GenerateCategory();
        var subCategory = ModelHelpers.SubCategory.GenerateSubCategory(categoryId: category.Id);
        var requestBody = ModelHelpers.Product.GenerateCreateRequestBody(category.Name!, subCategory.Name!);

        var entity = requestBody!.ToCreateProduct();
        entity.Id = CommonHelpers.GenerateBsonId();
        entity.CategoryId = category.Id;
        entity.SubCategoryId = subCategory.Id;
        
        var productRepository = new Mock<IRepository<Product>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryRepository = new Mock<IRepository<SubCategory>>();

        productRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>(), default)).ReturnsAsync(false);
        productRepository.Setup(x => x.CreateEntityAsync(It.IsAny<Product>(), default)).ReturnsAsync(entity);
        categoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(category);
        subCategoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(subCategory);
        
        IProductService service = new ProductService(productRepository.Object, categoryRepository.Object, subCategoryRepository.Object);

        var result = await service.CreateProductAsync(requestBody!);
        
        Assert.NotNull(result);
        Assert.True(result.IsSuccessCode);
        Assert.NotNull(result.Data);
        
        Assert.True(!string.IsNullOrWhiteSpace(result.Data.Id));
        // Assert.Equal(result.Data.Category, category.Name);
        // Assert.Equal(result.Data.SubCategory, subCategory.Name);
    }

    #endregion
}