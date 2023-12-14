using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Catalog.Models.SubCategory;
using ApiClient.Common;
using Catalog.API.Common.Consts;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Repositories;
using System.Threading;
using static Catalog.API.Common.Consts.ResponseMessages;
using SubCategory = Catalog.API.Entities.SubCategory;

namespace Catalog.API.Services
{
    public interface ISubCategoryService
    {
        Task<List<SubCategorySummary>> GetSubCategoriesAsync(CancellationToken cancellationToken = default);
        Task<SubCategorySummary> GetSubCategoryByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<SubCategorySummary> GetSubCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
        Task <ApiStatusResult> DeleteSubCategoryAsync(string id, CancellationToken cancellationToken = default);
        Task <ApiDataResult<SubCategorySummary>> CreateSubCategoryAsync (CreateSubCategoryRequestBody body, CancellationToken cancellationToken = default);
        Task<ApiDataResult<SubCategorySummary>> UpdateSubCategoryAsync(UpdateSubCategoryRequestBody body, CancellationToken cancellationToken = default);
    }
    public class SubCategoryService : ISubCategoryService
    {
        private readonly IRepository<SubCategory> _subCategoryRepository;
        private readonly IRepository<Category> _Category;
        public SubCategoryService(IRepository<SubCategory> subCategoryRepository)
        {
            _subCategoryRepository = subCategoryRepository;
        }

        public async Task<List<SubCategorySummary>> GetSubCategoriesAsync(CancellationToken cancellationToken)
        {
            var entities = await _subCategoryRepository.GetEntitiesAsync(cancellationToken);

            var subCategoryList = new List<SubCategorySummary>();

            foreach (var entity in entities)
            {
                subCategoryList.Add(entity.ToSummary());
            }

            return subCategoryList;
        }

        public async Task<SubCategorySummary> GetSubCategoryByNameAsync (string name, CancellationToken cancellationToken)
        {
            var subCategoryByName = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.Name == name, cancellationToken);

            return subCategoryByName.ToSummary();
        }

        public async Task<SubCategorySummary> GetSubCategoryByIdAsync(string id, CancellationToken cancellationToken)
        {
            var subCategoryByName = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return subCategoryByName.ToSummary();
        }

        public async Task<ApiStatusResult> DeleteSubCategoryAsync (string name, CancellationToken cancellationToken)
        {
            var subcategory = _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.Name == name, cancellationToken);

            var outcome = new ApiStatusResult();

            if (subcategory is null)
            {
                outcome.Message = ResponseMessages.Delete.NotFound;

                return outcome;
            }

            var result = await _subCategoryRepository.DeleteEntityAsync(name, cancellationToken);

            if (result == false)
            {
                outcome.Message = ResponseMessages.Delete.DeleteFailed;
            }

            return outcome;
        }

        public async Task<ApiDataResult<SubCategorySummary>> CreateSubCategoryAsync(CreateSubCategoryRequestBody requestBody, CancellationToken cancellationToken)
        {
            var apiDataResult = new ApiDataResult<SubCategorySummary>();

            var isNameExisted = await _subCategoryRepository.AnyAsync(x => x.Name == requestBody.Name, cancellationToken);

            if(isNameExisted)
            {
                apiDataResult.Message = ResponseMessages.SubCategory.SubCategoryExisted(requestBody.Name);          
                return apiDataResult;
            } else
            {
                var isCodeExisted = await _subCategoryRepository.AnyAsync(x => x.SubCategoryCode == requestBody.SubCategoryCode, cancellationToken);
                if(isCodeExisted)
                {
                    apiDataResult.Message = ResponseMessages.SubCategory.SubCategoryCodeExisted(requestBody.SubCategoryCode);
                    return apiDataResult;
                }    
            }    

            var isCategoryIdAvailable = await _subCategoryRepository.AnyAsync(x => x.CategoryId == requestBody.CategoryId, cancellationToken);
            

        }


    }

}

