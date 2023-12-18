using ApiClient.Catalog.Models.SubCategory;
using ApiClient.Common;
using Catalog.API.Common.Consts;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Repositories;
using System.Threading;
using static Catalog.API.Common.Consts.ResponseMessages;
using Category = Catalog.API.Entities.Category;
using SubCategory = Catalog.API.Entities.SubCategory;

namespace Catalog.API.Services
{
    public interface ISubCategoryService
    {
        Task<ApiDataResult<List<SubCategorySummary>>> GetSubCategoriesAsync(CancellationToken cancellationToken = default);
        Task<ApiDataResult<SubCategorySummary>> GetSubCategoryByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<ApiDataResult<SubCategorySummary>> GetSubCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<ApiDataResult<List<SubCategorySummary>>> GetSubCategoryByCategoryIdAsync (string categoryId, CancellationToken cancellationToken = default);
        Task <ApiStatusResult> DeleteSubCategoryAsync(string id, CancellationToken cancellationToken = default);
        Task <ApiDataResult<SubCategorySummary>> CreateSubCategoryAsync (CreateSubCategoryRequestBody body, CancellationToken cancellationToken = default);
        Task<ApiDataResult<SubCategorySummary>> UpdateSubCategoryAsync(UpdateSubCategoryRequestBody body, CancellationToken cancellationToken = default);
    }
    public class SubCategoryService : ISubCategoryService
    {
        private readonly IRepository<SubCategory> _subCategoryRepository;
        private readonly IRepository<Category> _category;
        public SubCategoryService(IRepository<SubCategory> subCategoryRepository)
        {
            _subCategoryRepository = subCategoryRepository;
        }

        public async Task<ApiDataResult<List<SubCategorySummary>>> GetSubCategoriesAsync(CancellationToken cancellationToken)
        {
            var apiDataResult = new ApiDataResult<List<SubCategorySummary>>()
            {
                Data = new List<SubCategorySummary>()
            };

            var entities = await _subCategoryRepository.GetEntitiesAsync(cancellationToken);

            if(entities != null)
            {
                foreach (var entity in entities)
                {
                    apiDataResult.Data.Add(entity.ToSummary());
                }

                return apiDataResult;
            }    

            apiDataResult.Message = ResponseMessages.SubCategory.NotFound;
            return apiDataResult;
            
        }

        public async Task<ApiDataResult<SubCategorySummary>> GetSubCategoryByNameAsync (string name, CancellationToken cancellationToken)
        {
            var apiDataResult = new ApiDataResult<SubCategorySummary>();

            var subCategoryByName = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.Name == name, cancellationToken);

            if(subCategoryByName == null)
            {
                apiDataResult.Message = ResponseMessages.SubCategory.NotFound;
                return apiDataResult;
            }    

            apiDataResult.Data = subCategoryByName.ToSummary();
            
            return apiDataResult;
        }

        public async Task<ApiDataResult<SubCategorySummary>> GetSubCategoryByIdAsync(string id, CancellationToken cancellationToken)
        {
            var apiDataResult = new ApiDataResult<SubCategorySummary>();

            var subCategoryById = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (subCategoryById == null)
            {
                apiDataResult.Message = ResponseMessages.SubCategory.NotFound;
                return apiDataResult;
            }
            apiDataResult.Data = subCategoryById.ToSummary();

            return apiDataResult;
        }

        public async Task<ApiDataResult<List<SubCategorySummary>>> GetSubCategoryByCategoryIdAsync(string categoryId, CancellationToken cancellationToken)
        {
            var apiDataResult = new ApiDataResult<List<SubCategorySummary>>()
            {
                Data = new List<SubCategorySummary>()
            };
            //check CategoryID trong Category

            //var isExisted = await _category.AnyAsync(x => x.Id == categoryId, cancellationToken);
            //if(!isExisted)
            //{
            //    apiDataResult.Message = ResponseMessages.SubCategory.NotFound;
            //    return apiDataResult;
            //}   
            
            var entities = await _subCategoryRepository.GetEntitiesAsync(cancellationToken);

            if(entities is null)
            {
                apiDataResult.Message = ResponseMessages.SubCategory.NotFound;
                return apiDataResult;
            }

            var filter = entities.Where(x => x.CategoryId == categoryId).ToList();

            foreach(var item in filter)
            {

                apiDataResult.Data.Add(item.ToSummary());
            }

            return apiDataResult;


        }

        public async Task<ApiStatusResult> DeleteSubCategoryAsync (string id, CancellationToken cancellationToken)
        {
            var subcategory = _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            var outcome = new ApiStatusResult();

            if (subcategory is null)
            {
                outcome.Message = ResponseMessages.Delete.NotFound;

                return outcome;
            }

            var result = await _subCategoryRepository.DeleteEntityAsync(id, cancellationToken);

            if (result == false)
            {
                outcome.Message = ResponseMessages.Delete.DeleteFailed;
            }

            return outcome;
        }

        public async Task<ApiDataResult<SubCategorySummary>> CreateSubCategoryAsync(CreateSubCategoryRequestBody requestBody, CancellationToken cancellationToken)
        {
            var apiDataResult = new ApiDataResult<SubCategorySummary>();

            var isExisted = await _subCategoryRepository.AnyAsync(x => x.Name == requestBody.Name || x.SubCategoryCode == requestBody.SubCategoryCode, cancellationToken);

            if(isExisted)
            {
                apiDataResult.Message = ResponseMessages.SubCategory.SubCategoryExisted;          
                return apiDataResult;
            } 

            var isCategoryIdAvailable = await _subCategoryRepository.AnyAsync(x => x.CategoryId == requestBody.CategoryId, cancellationToken);
            
            if(!isCategoryIdAvailable)
            {
                apiDataResult.Message = ResponseMessages.SubCategory.CategoryIdNotFound(requestBody.CategoryId);
                return apiDataResult;
            }

            var subcategory = requestBody.ToCreateSubCategory();
            await _subCategoryRepository.CreateEntityAsync(subcategory, cancellationToken);

            if(string.IsNullOrWhiteSpace(subcategory.Id))
            {
                apiDataResult.Message = ResponseMessages.SubCategory.CreateSubCategoryFailed;
            }
            // add data
            apiDataResult.Data = subcategory.ToSummary();

            return apiDataResult;

        }

        public async Task<ApiDataResult<SubCategorySummary>> UpdateSubCategoryAsync(UpdateSubCategoryRequestBody body, CancellationToken cancellationToken)
        {
            var apiDataResult = new ApiDataResult<SubCategorySummary>();

            var subCategory = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == body.Id , cancellationToken);

            if(subCategory is null)
            {
               apiDataResult.Message = ResponseMessages.SubCategory.NotFound;
            }
            else
            {
                var isNameExisted = await _subCategoryRepository.AnyAsync(x => x.Name == body.Name, cancellationToken);
                if (isNameExisted)
                {
                    var isIdExisted = await _subCategoryRepository.AnyAsync(x => x.Id == body.Id, cancellationToken);
                    if (isIdExisted)
                    {
                        subCategory.ToUpdateSubCategory(body);

                        var updateItem = await _subCategoryRepository.UpdateEntityAsync(subCategory, cancellationToken);

                        if (!updateItem)
                        {
                            apiDataResult.Message = ResponseMessages.SubCategory.UpdateSubCategoryFailed;
                            return apiDataResult;
                        }

                        apiDataResult.Data = subCategory.ToSummary();
                        return apiDataResult;
                    }
                    apiDataResult.Message = ResponseMessages.SubCategory.NotFound;
                    return apiDataResult;
                }
                else
                {
                    var isCodeExisted = await _subCategoryRepository.AnyAsync(x => x.SubCategoryCode == body.SubCategoryCode, cancellationToken);
                    if (isCodeExisted)
                    {
                        apiDataResult.Message = ResponseMessages.SubCategory.SubCategoryCodeExisted;
                        return apiDataResult;
                    }
                }
            }

            subCategory.ToUpdateSubCategory(body);

            var result = await _subCategoryRepository.UpdateEntityAsync(subCategory, cancellationToken);

            if (!result)
            {
                apiDataResult.Message = ResponseMessages.SubCategory.UpdateSubCategoryFailed;
                return apiDataResult;
            }

            apiDataResult.Data = subCategory.ToSummary();
            return apiDataResult;
        }

    }

}

