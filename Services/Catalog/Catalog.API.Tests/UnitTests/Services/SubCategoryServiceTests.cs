
using Catalog.API.Common.Consts;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Repositories;
using Catalog.API.Services;
using Catalog.API.Tests.Common;
using Moq;
using System.Linq.Expressions;


//using UnitTest.Common.Helpers;


namespace Catalog.API.Tests.UnitTests.Services;
public class SubCategoryServiceTests
{
    #region GetSubCategoryAsync
    [Fact]
    public async Task GetSubCategoriesAsync_ValidParams_ExpectedResult()
    {
        var subCategories = ModelHelpers.SubCategory.GenerateSubCategories();
        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object);

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
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object);

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
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object);

        subCategoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync(entity);

        var result = await subCategoryService.GetSubCategoryByNameAsync(entity.Name, default);

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
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object);

        subCategoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<SubCategory, bool>>>(), default)).ReturnsAsync((SubCategory)null!);

        var result = await subCategoryService.GetSubCategoryByNameAsync("name", default);

        Assert.Null(result.Data);
        Assert.Equal(ResponseMessages.SubCategory.NotFound, result.Message);
    }
    #endregion

    #region SubCategoryByID
    [Fact]
    public async Task GetSubCategoryByIdAsync_ValidParams_ExpectedResult()
    {
        var entity = ModelHelpers.SubCategory.GenerateSubCategory();
        var subCategoryDetail = entity.ToDetail();
        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object);

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
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object);

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
        var subCategories = ModelHelpers.SubCategory.GenerateSubCategories();
        var subCategoryRepository = new Mock<IRepository<SubCategory>>();
        var subCategoryService = new SubCategoryService(subCategoryRepository.Object);

        subCategoryRepository.Setup(x => x.GetEntitiesAsync(default)).ReturnsAsync(subCategories);

        var result = await subCategoryService.GetSubCategoryByNameAsync("123456", default);


        Assert.NotNull(result.Data);
        Assert.Null(result.Message);
    }
        #endregion

        #region

        #endregion
}
