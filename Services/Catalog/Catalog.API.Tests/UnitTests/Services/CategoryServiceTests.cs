using Catalog.API.Common.Consts;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Repositories;
using Catalog.API.Services;
using Catalog.API.Tests.Common;
using System.Linq.Expressions;
using UnitTest.Common.Helpers;

namespace Catalog.API.Tests.UnitTests.Services;

public class CategoryServiceTests
{
    [Collection("CategoryServiceTests")]
    public class GetCategoriesAsync
    {
        [Fact]
        public async Task GetCategoriesAsync_ValidParams_ExpectedResult()
        {
            var categories = ModelHelpers.Category.GenerateCategories();
            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.GetEntitiesAsync(default)).ReturnsAsync(categories);

            var result = await categoryService.GetCategoriesAsync(default);

            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data);
            Assert.Equal(categories.Count, result.Data.Count);
            Assert.Null(result.Message);
        }

        [Fact]
        public async Task GetCategoriesAsync_InvalidParams_NotFound()
        {
            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.GetEntitiesAsync(default)).ReturnsAsync((List<Category>)null!);

            var result = await categoryService.GetCategoriesAsync(default);

            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            Assert.Equal(ResponseMessages.Category.NotFound, result.Message);
        }
    }

    [Collection("CategoryServiceTests")]
    public class GetCategoryByName
    {
        [Fact]
        public async Task GetCategoryByNameAsync_ValidParams_ExpectedResult()
        {
            var entity = ModelHelpers.Category.GenerateCategoryEntity();
            var categoryDetail = entity.ToDetail();
            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(entity);

            var result = await categoryService.GetCategoryByNameAsync(entity.Name!, default);

            Assert.NotNull(result.Data);
            Assert.Equal(categoryDetail.Name, result.Data.Name);
            Assert.Equal(categoryDetail.Id, result.Data.Id);
            Assert.Equal(categoryDetail.Description, result.Data.Description);
            Assert.Equal(categoryDetail.CategoryCode, result.Data.CategoryCode);
            Assert.Null(result.Message);
        }

        [Fact]
        public async Task GetCategoryByNameASync_InvalidParams_NotFound()
        {
            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync((Category)null!);

            var result = await categoryService.GetCategoryByNameAsync("name", default);

            Assert.Null(result.Data);
            Assert.Equal(ResponseMessages.Category.NotFound, result.Message);
        }
    }

    [Collection("CategoryServiceTests")]
    public class GetCategoryById
    {
        [Fact]
        public async Task GetCategoryByIdAsync_ValidParams_ExpectedResult()
        {
            var entity = ModelHelpers.Category.GenerateCategoryEntity();
            var categoryDetail = entity.ToDetail();
            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(entity);

            var result = await categoryService.GetCategoryByNameAsync(entity.Id, default);

            Assert.NotNull(result.Data);
            Assert.Equal(categoryDetail.Name, result.Data.Name);
            Assert.Equal(categoryDetail.Id, result.Data.Id);
            Assert.Equal(categoryDetail.Description, result.Data.Description);
            Assert.Equal(categoryDetail.CategoryCode, result.Data.CategoryCode);
            Assert.Null(result.Message);
        }

        [Fact]
        public async Task GetCategoryByIdASync_InvalidParams_NotFound()
        {
            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync((Category)null!);

            var result = await categoryService.GetCategoryByNameAsync("id", default);

            Assert.Null(result.Data);
            Assert.Equal(ResponseMessages.Category.NotFound, result.Message);
        }
    }

    [Collection("CategoryServiceTest")]
    public class CreateCategory
    {
        [Fact]
        public async Task CreateCategoryAsync_ValidParams_ExpectedResult()
        {
            var requestBody = ModelHelpers.Category.GenerateCreateRequestBody();
            var entity = requestBody.ToCreateCategory();
            entity.Id = CommonHelpers.GenerateBsonId();

            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(false);
            categoryRepository.Setup(x => x.CreateEntityAsync(It.IsAny<Category>(), default)).ReturnsAsync(entity);

            var result = await categoryService.CreateCategoryAsync(requestBody, default);

            Assert.NotNull(result);
            Assert.Null(result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(requestBody.Name, result.Data.Name);
            Assert.Equal(requestBody.CategoryCode, result.Data.CategoryCode);
            Assert.Equal(requestBody.Description, result.Data.Description);
            Assert.True(result.IsSuccessCode);
        }

        [Fact]
        public async Task CreateCategoryAsync_ValidParams_Existed()
        {
            var requestBody = ModelHelpers.Category.GenerateCreateRequestBody();

            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(true);

            var result = await categoryService.CreateCategoryAsync(requestBody, default);

            Assert.Equal(ResponseMessages.Category.CategoryExisted(requestBody.Name, requestBody.CategoryCode), result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task CreateCategoryAsync_ValidParams_SaveFailure()
        {
            var requestBody = ModelHelpers.Category.GenerateCreateRequestBody();

            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            var entity = requestBody.ToCreateCategory();

            categoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(false);

            categoryRepository.Setup(x => x.CreateEntityAsync(It.IsAny<Category>(), default)).ReturnsAsync(entity);

            var result = await categoryService.CreateCategoryAsync(requestBody, default);

            Assert.Equal(ResponseMessages.Category.CreateFailed, result.Message);
            Assert.Null(result.Data);
        }
    }

    [Collection("CategoryServiceTest")]
    public class UpdateCategory {
        [Fact]
        public async Task UpdateCategoryAsync_ValidParams_ExpectedResult()
        {
            var id = CommonHelpers.GenerateBsonId();
            var requestBody = ModelHelpers.Category.GenerateUpdateRequestBody(id);
            var entity = ModelHelpers.Category.GenerateCategoryEntity(id);

            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(entity);

            categoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(false); 

            categoryRepository.Setup(x => x.UpdateEntityAsync(It.IsAny<Category>(), default)).ReturnsAsync(true); 

            var result = await categoryService.UpdateCategoryAsync(requestBody, default);

            Assert.NotNull(result);
            Assert.Null(result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(requestBody.Id, result.Data.Id);
            Assert.Equal(requestBody.Name, result.Data.Name);
            Assert.Equal(requestBody.CategoryCode, result.Data.CategoryCode);
            Assert.Equal(requestBody.Description, result.Data.Description);
            Assert.True(result.IsSuccessCode);
        }

        [Fact]
        public async Task UpdateCategoryAsync_InvalidParams_NotFound()
        {
            var requestBody = ModelHelpers.Category.GenerateUpdateRequestBody();

            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync((Category)null!);

            var result = await categoryService.UpdateCategoryAsync(requestBody, default);

            Assert.NotNull(result);
            Assert.Equal(ResponseMessages.Category.NotFound, result.Message);
            Assert.False(result.IsSuccessCode);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ValidParams_Existed()
        {
            var entity = ModelHelpers.Category.GenerateCategoryEntity();
            var requestBody = ModelHelpers.Category.GenerateUpdateRequestBody();

            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(entity);

            categoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(true);

            var result = await categoryService.UpdateCategoryAsync(requestBody, default);

            Assert.NotNull(result);
            Assert.Equal(ResponseMessages.Category.CategoryExisted(requestBody.Name, requestBody.CategoryCode), result.Message);
            Assert.False(result.IsSuccessCode);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ValidParams_SaveFailure()
        {
            var entity = ModelHelpers.Category.GenerateCategoryEntity();
            var requestBody = ModelHelpers.Category.GenerateUpdateRequestBody();

            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(entity);

            categoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(false);

            categoryRepository.Setup(x => x.UpdateEntityAsync(It.IsAny<Category>(), default)).ReturnsAsync(false);

            var result = await categoryService.UpdateCategoryAsync(requestBody, default);

            Assert.NotNull(result);
            Assert.Equal(ResponseMessages.Category.UpdateFailed, result.Message);
            Assert.False(result.IsSuccessCode);
        }
    }

    [Collection("CategoryServiceTest")]
    public class DeleteCategory
    {
        [Fact]
        public async Task DeleteCategoryAsync_ValidParams_ExpectedResult()
        {
            var categoryId = "id";
            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(true);

            categoryRepository.Setup(x => x.DeleteEntityAsync(categoryId, default)).ReturnsAsync(true);

            var result = await categoryService.DeleteCategoryAsync(categoryId, default);

            Assert.NotNull(result);
            Assert.Null(result.Message);
            Assert.True(result.IsSuccessCode);
        }

        [Fact]
        public async Task DeleteCategoryAsync_InvalidParams_NotFound()
        {
            var CategoryId = "id";
            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(false);

            var result = await categoryService.DeleteCategoryAsync(CategoryId, default);

            Assert.NotNull(result);
            Assert.Equal(ResponseMessages.Category.NotFound, result.Message);
            Assert.False(result.IsSuccessCode);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ValidParams_SaveFailure()
        {
            var categoryId = "id";
            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(true);

            categoryRepository.Setup(x => x.DeleteEntityAsync(categoryId, default)).ReturnsAsync(false);

            var result = await categoryService.DeleteCategoryAsync(categoryId, default);

            Assert.NotNull(result);
            Assert.Equal(ResponseMessages.Category.DeleteFailed, result.Message);
            Assert.False(result.IsSuccessCode);
        }
    }
}
