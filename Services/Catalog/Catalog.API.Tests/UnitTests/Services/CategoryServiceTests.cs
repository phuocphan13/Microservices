using ApiClient.Catalog.Category.Models;
using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Catalog.Product.Models;
using ApiClient.Common;
using Catalog.API.Common.Consts;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Repositories;
using Catalog.API.Services;
using Catalog.API.Tests.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharpCompress.Common;
using System.Linq.Expressions;
using System.Net;
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
            var category = ModelHelpers.Category.GenerateCategory();
            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(category);

            var result = await categoryService.GetCategoryByNameAsync(category.Name!, default);

            Assert.NotNull(result.Data);
            Assert.Equivalent(category.ToDetail(), result.Data);
            //Assert.Equal(category.ToDetail().Name, result.Data.Name);
            //Assert.Equal(category.ToDetail().Id, result.Data.Id);
            //Assert.Equal(category.ToDetail().Description, result.Data.Description);
            //Assert.Equal(category.ToDetail().CategoryCode, result.Data.CategoryCode);
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
            var category = ModelHelpers.Category.GenerateCategory();
            var categoryRepository = new Mock<IRepository<Category>>();
            var categoryService = new CategoryService(categoryRepository.Object);

            categoryRepository.Setup(x => x.GetEntityFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), default)).ReturnsAsync(category);

            var result = await categoryService.GetCategoryByNameAsync(category.Id!, default);

            Assert.NotNull(result.Data);
            Assert.Equal(category.ToDetail().Name, result.Data.Name);
            Assert.Equal(category.ToDetail().Id, result.Data.Id);
            Assert.Equal(category.ToDetail().Description, result.Data.Description);
            Assert.Equal(category.ToDetail().CategoryCode, result.Data.CategoryCode);
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
    }
}
