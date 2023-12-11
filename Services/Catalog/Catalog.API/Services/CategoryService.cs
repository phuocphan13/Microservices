using ApiClient.Catalog.Models.Category;
using ApiClient.Catalog.Models.Product;
using ApiClient.Common;
using Catalog.API.Common.Consts;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Repositories;

namespace Catalog.API.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default);
        Task<ApiDataResult<Category>> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
        Task<Category> GetCategoryByNameAsync(string categoryName, CancellationToken cancellationToken = default);
        Task<Category> GetCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<ApiDataResult<Category>> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
    }

    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken)
        {
            var categoryList = await _categoryRepository.GetEntitiesAsync(cancellationToken);

            return categoryList;
        }

        public async Task<Category> GetCategoryByNameAsync(string categoryName, CancellationToken cancellationToken)
        {
            var categoryByName = await _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.Name == categoryName, cancellationToken);

            return categoryByName;
        }

        public async Task<Category> GetCategoryByIdAsync(string categoryId, CancellationToken cancellationToken)
        {
            var categoryById = await _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == categoryId, cancellationToken);

            return categoryById;
        }

        public async Task<ApiDataResult<Category>> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken)
        {
            var apiDataResult = new ApiDataResult<Category>();

            var isExisted = await _categoryRepository.AnyAsync(x => x.Name == requestBody.Name, cancellationToken);

            if (isExisted)
            {
                apiDataResult.Message = ResponseMessages.Category.CategoryExisted(requestBody.Name);
                return apiDataResult;
            }

            var category = requestBody.ToCreateCategory();

            await _categoryRepository.CreateEntityAsync(category, cancellationToken);

            if (string.IsNullOrWhiteSpace(category.Id))
            {
                apiDataResult.Message = ResponseMessages.Category.CreateFailed;
                return apiDataResult;
            }

            apiDataResult.Data = category;
            return apiDataResult;
        }

        public async Task<ApiDataResult<Category>> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken)
        {
            var apiDataResult = new ApiDataResult<Category>();
            var category = await _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.Name == requestBody.Name, cancellationToken);

            if (category is null)
            {
                apiDataResult.Message = ResponseMessages.Category.NotFound;
                return apiDataResult;
            }

            category.ToUpdateCategory(requestBody);

            var result = await _categoryRepository.UpdateEntityAsync(category, cancellationToken);

            if (!result)
            {
                apiDataResult.Message = ResponseMessages.Product.UpdateFailed;
                return apiDataResult;
            }

            apiDataResult.Data = category;
            return apiDataResult;
        }
    }
}
