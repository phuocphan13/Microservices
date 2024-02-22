using Catalog.API.Common.Consts;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Repositories;
using Catalog.API.Services;
using Catalog.API.Tests.Common;
using System.Linq.Expressions;
using UnitTest.Common.Helpers;

namespace Catalog.API.Tests.UnitTests.Services;

public class SubCategoryServiceTests
{
    #region GetSubCategoryAsync
    [Fact]
    public async Task GetSubCategoriesAsync_ValidParams_ExpectedResult()
    {
        var subCategories = ModelHelpers.SubCategory.GenerateSubCategories();
        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object, categoryRepository.Object);

        subCategoryRepository.Setup(x => x.GetEntitiesAsync(default)).ReturnsAsync(subCategories);

        var result = await subCategoryService.GetSubCategoriesAsync(default);

        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data);
        Assert.Equal(subCategories.Count, result.Data.Count);
        Assert.Null(result.Message);
    }

    [Fact]
    public async Task GetSubCategoriesAsync_InvalidParams_NotFound()
    {
        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object, categoryRepository.Object);

        subCategoryRepository.Setup(x => x.GetEntitiesAsync(default)).ReturnsAsync((List<SubCategory>)null!);

        var result = await subCategoryService.GetSubCategoriesAsync(default);

        Assert.NotNull(result.Data);
        Assert.Empty(result.Data);
        Assert.Equal(ResponseMessages.SubCategory.NotFound, result.Message);
    }
    #endregion

    #region GetSubCategoryByNameAsync
    [Fact]
    public async Task GetSubCategoryByNameAsync_ValidParams_ExpectedResult()
    {
        var entity = ModelHelpers.SubCategory.GenerateSubCategory();
        var subCategoryDetail = entity.ToDetail();
        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object, categoryRepository.Object);

        subCategoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(entity);

        var result = await subCategoryService.GetSubCategoryByNameAsync(entity.Name!, default);

        Assert.NotNull(result.Data);
        Assert.Equal(subCategoryDetail.Name, result.Data.Name);
        Assert.Equal(subCategoryDetail.Id, result.Data.Id);
        Assert.Equal(subCategoryDetail.CategoryId, result.Data.CategoryId);
        Assert.Equal(subCategoryDetail.Description, result.Data.Description);
        Assert.Equal(subCategoryDetail.SubCategoryCode, result.Data.SubCategoryCode);
        Assert.Null(result.Message);
    }

    [Fact]
    public async Task GetSubCategoryByNameAsync_InvalidParams_NotFound()
    {
        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object, categoryRepository.Object);

        subCategoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync((SubCategory)null!);

        var result = await subCategoryService.GetSubCategoryByNameAsync("name", default);

        Assert.Null(result.Data);
        Assert.Equal(ResponseMessages.SubCategory.NotFound, result.Message);
    }
    #endregion

    #region SubCategoryById
    [Fact]
    public async Task GetSubCategoryByIdAsync_ValidParams_ExpectedResult()
    {
        var entity = ModelHelpers.SubCategory.GenerateSubCategory();
        var subCategoryDetail = entity.ToDetail();
        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object, categoryRepository.Object);

        subCategoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(entity);

        var result = await subCategoryService.GetSubCategoryByNameAsync(entity.Id, default);

        Assert.NotNull(result.Data);
        Assert.Equal(subCategoryDetail.Name, result.Data.Name);
        Assert.Equal(subCategoryDetail.Id, result.Data.Id);
        Assert.Equal(subCategoryDetail.CategoryId, result.Data.CategoryId);
        Assert.Equal(subCategoryDetail.Description, result.Data.Description);
        Assert.Equal(subCategoryDetail.SubCategoryCode, result.Data.SubCategoryCode);
        Assert.Null(result.Message);
    }

    [Fact]
    public async Task GetSubCategoryByIdAsync_InvalidParams_NotFound()
    {
        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object, categoryRepository.Object);

        subCategoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync((SubCategory)null!);

        var result = await subCategoryService.GetSubCategoryByNameAsync("id", default);

        Assert.Null(result.Data);
        Assert.Equal(ResponseMessages.SubCategory.NotFound, result.Message);
    }
    #endregion

    #region GetSubCategoriesByCategoryIdAsync
    [Fact]
    public async Task GetSubCategoriesByCategoryIdAsync_ValidParams_ExpectedResult()
    {
        var categoryId = CommonHelpers.GenerateBsonId();

        var subCategories = ModelHelpers.SubCategory.GenerateSubCategories(initAction: x =>
        {
            x.CategoryId = categoryId;
        });

        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object, categoryRepository.Object);

        subCategoryRepository.Setup(x => x.GetEntitiesQueryAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(subCategories);

        var result = await subCategoryService.GetSubCategoriesByCategoryIdAsync(categoryId, default);

        Assert.NotNull(result.Data);
        Assert.Null(result.Message);
    }

    [Fact]
    public async Task GetSubCategoriesByCategoryIdAsync_InvalidParams_NotFound()
    {
        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object, categoryRepository.Object);

        subCategoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync((SubCategory)null!);

        var result = await subCategoryService.GetSubCategoryByNameAsync("id", default);

        Assert.Null(result.Data);
        Assert.Equal(ResponseMessages.SubCategory.NotFound, result.Message);
    }
    #endregion

    #region CreateSubCategory
    [Fact]
    public async Task CreateSubCategoryAsync_ValidParams_ExpectedResult()
    {
        var requestBody = ModelHelpers.SubCategory.GenerateCreateRequestBody();

        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object, categoryRepository.Object);

        subCategoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(false);

        var result = await subCategoryService.CreateSubCategoryAsync(requestBody, default);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateSubCategoryAsync_InvalidParams_Existed()
    {
        var requestBody = ModelHelpers.SubCategory.GenerateCreateRequestBody();

        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCatergoryService = new SubCategoryService(subCategoryRepository.Object, categoryRepository.Object);

        subCategoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(true);

        var result = await subCatergoryService.CreateSubCategoryAsync(requestBody, default);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateSubCategoryAsync_ValidParams_CategoryIdNotFound()
    {
        var categoryId = CommonHelpers.GenerateBsonId();
        var categoryRepository = new Mock<IRepository<Category>>();

        var requestBody = ModelHelpers.SubCategory.GenerateCreateRequestBody(initAction: x =>
        {
            x.CategoryId = categoryId;
        });

        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object, categoryRepository.Object);

        subCategoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(false);
        categoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(false);

        var result = await subCategoryService.CreateSubCategoryAsync(requestBody, default);

        Assert.Equal(result.Message, ResponseMessages.SubCategory.CategoryIdNotFound(categoryId));
    }
    #endregion

    #region UpdateSupcatergory
    [Fact]
    public async Task UpdateSupcatergory_ValidParams_Existed()
    {
        var requestBody = ModelHelpers.SubCategory.GenerateUpdateRequestBody();
        var entity = ModelHelpers.SubCategory.GenerateSubCategoryEntity();

        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object, categoryRepository.Object);

        subCategoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(entity);
        subCategoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync((true));

        var result = await subCategoryService.UpdateSubCategoryAsync(requestBody, default);

        Assert.NotNull(result);
        Assert.Equal(ResponseMessages.SubCategory.SubCategoryExisted, result.Message);
        Assert.False(result.IsSuccessCode);
    }

    [Fact]
    public async Task UpdateSupcatergory_ValidParams_SubCategoryNotFound()
    {
        var requestBody = ModelHelpers.SubCategory.GenerateUpdateRequestBody();

        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object, categoryRepository.Object);

        subCategoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync((SubCategory)null!);

        var result = await subCategoryService.UpdateSubCategoryAsync(requestBody, default);

        Assert.NotNull(result);
        Assert.Equal(ResponseMessages.SubCategory.NotFound, result.Message);
        Assert.False(result.IsSuccessCode);
    }

    [Fact]
    public async Task UpdateSupcatergory_ValidParams_SaveFailure()
    {
        var requestBody = ModelHelpers.SubCategory.GenerateUpdateRequestBody();
        var entity = ModelHelpers.SubCategory.GenerateSubCategoryEntity();

        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object, categoryRepository.Object);

        subCategoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(entity);
        subCategoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync((false));
        subCategoryRepository.Setup(x => x.UpdateEntityAsync(It.IsAny<SubCategory>(), default)).ReturnsAsync((false));

        var result = await subCategoryService.UpdateSubCategoryAsync(requestBody, default);

        Assert.NotNull(result);
        Assert.Equal(ResponseMessages.SubCategory.UpdateSubCategoryFailed, result.Message);
        Assert.False(result.IsSuccessCode);
    }

    [Fact]
    public async Task UpdateSupcatergory_ValidParams_ExpectedResult()
    {
        var id = CommonHelpers.GenerateBsonId();
        var requestBody = ModelHelpers.SubCategory.GenerateUpdateRequestBody(id);
        var entity = ModelHelpers.SubCategory.GenerateSubCategoryEntity(id);

        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var categoryRepository = new Mock<IRepository<Category>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object, categoryRepository.Object);

        subCategoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(entity);
        subCategoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync((false));
        subCategoryRepository.Setup(x => x.UpdateEntityAsync(It.IsAny<SubCategory>(), default)).ReturnsAsync((false));

        var result = await subCategoryService.UpdateSubCategoryAsync(requestBody, default);

        Assert.NotNull(result);
        Assert.False(result.IsSuccessCode);
    }
    #endregion
}