using Catalog.API.Entities;
using Catalog.API.Repositories;

namespace Catalog.API.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default);
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
            var categoryList = await GetCategoriesInternalAsync(cancellationToken);

            return categoryList;
        }
        #region Internal Functions
        private async Task<List<Category>> GetCategoriesInternalAsync(CancellationToken cancellationToken)
        {
            var categoryEntities = await _categoryRepository.GetEntitiesAsync(cancellationToken);

            return categoryEntities;
        }
        #endregion
    }
}
