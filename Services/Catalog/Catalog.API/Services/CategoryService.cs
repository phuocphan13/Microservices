using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Common;
using Catalog.API.Common.Consts;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Repositories;

namespace Catalog.API.Services
{
    public interface ICategoryService
    {
        Task<List<CategorySummary>> GetCategoriesAsync(CancellationToken cancellationToken = default);
        Task<ApiDataResult<CategorySummary>> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
        Task<CategorySummary> GetCategoryByNameAsync(string categoryName, CancellationToken cancellationToken = default);
        Task<CategorySummary> GetCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<ApiDataResult<CategorySummary>> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
        Task<ApiStatusResult> DeleteCategoryAsync(string categoryId, CancellationToken cancellationToken = default);
    }

    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategorySummary>> GetCategoriesAsync(CancellationToken cancellationToken)
        {
            var entities = await _categoryRepository.GetEntitiesAsync(cancellationToken);

            var categoryList = new List<CategorySummary>();
            
            foreach(var entity in entities)
            {
                categoryList.Add(entity.ToSummary());
            }

            return categoryList;
        }

        public async Task<CategorySummary> GetCategoryByNameAsync(string categoryName, CancellationToken cancellationToken)
        {
            var categoryByName = await _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.Name == categoryName, cancellationToken);

            return categoryByName.ToSummary();
        }

        public async Task<CategorySummary> GetCategoryByIdAsync(string categoryId, CancellationToken cancellationToken)
        {
            var categoryById = await _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == categoryId, cancellationToken);

            return categoryById.ToSummary();
        }

        public async Task<ApiDataResult<CategorySummary>> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken)
        {
            var apiDataResult = new ApiDataResult<CategorySummary>();

            var isNameExisted = await _categoryRepository.AnyAsync(x => x.Name == requestBody.Name, cancellationToken);

            if (isNameExisted)
            {
                apiDataResult.Message = ResponseMessages.Category.CategoryNameExisted(requestBody.Name);
                return apiDataResult;
            }else 
            {
                var isCodeExisted = await _categoryRepository.AnyAsync(x => x.CategoryCode == requestBody.CategoryCode, cancellationToken);
                if (isCodeExisted) {
                    apiDataResult.Message = ResponseMessages.Category.CategoryCodeExisted(requestBody.CategoryCode);
                    return apiDataResult;
                } 
            }

            var category = requestBody.ToCreateCategory();

            await _categoryRepository.CreateEntityAsync(category, cancellationToken);

            if (string.IsNullOrWhiteSpace(category.Id))
            {
                apiDataResult.Message = ResponseMessages.Category.CreateFailed;
                return apiDataResult;
            }

            apiDataResult.Data = category.ToSummary();
            return apiDataResult;
        }

        public async Task<ApiDataResult<CategorySummary>> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken)
        {
            var apiDataResult = new ApiDataResult<CategorySummary>();
            var category = await _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == requestBody.Id, cancellationToken);

            if (category is null)
            {
                apiDataResult.Message = ResponseMessages.Category.NotFound;
                return apiDataResult;
            }
            else
            {
                var isNameExisted = await _categoryRepository.AnyAsync(x => x.Name == requestBody.Name, cancellationToken);
                if (isNameExisted)
                {
                    apiDataResult.Message = ResponseMessages.Category.CategoryNameExisted(requestBody.Name);
                    return apiDataResult;
                }
                else
                {
                    var isCodeExisted = await _categoryRepository.AnyAsync(x => x.CategoryCode == requestBody.CategoryCode, cancellationToken);
                    if (isCodeExisted)
                    {
                        apiDataResult.Message = ResponseMessages.Category.CategoryCodeExisted(requestBody.CategoryCode);
                        return apiDataResult;
                    }
                }
            }

            category.ToUpdateCategory(requestBody);

            var result = await _categoryRepository.UpdateEntityAsync(category, cancellationToken);

            if (!result)
            {
                apiDataResult.Message = ResponseMessages.Product.UpdateFailed;
                return apiDataResult;
            }

            apiDataResult.Data = category.ToSummary();
            return apiDataResult;
        }

        public async Task<ApiStatusResult> DeleteCategoryAsync(string categoryId, CancellationToken cancellationToken)
        {
            var apiDataResult = new ApiStatusResult();
            var isExisted = await _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == categoryId, cancellationToken);

            if(isExisted is null)
            {
                apiDataResult.Message = ResponseMessages.Category.NotFound;
                return apiDataResult;
            }

            var result = await _categoryRepository.DeleteEntityAsync(categoryId, cancellationToken);

            if (!result)
            {
                apiDataResult.Message = ResponseMessages.Product.UpdateFailed;
                return apiDataResult;
            }

            return apiDataResult;
        }
    }
}
