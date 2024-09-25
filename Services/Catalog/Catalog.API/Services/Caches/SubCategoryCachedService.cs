using Catalog.API.Repositories;
using Platform.Database.Redis;
using Catalog.API.Models;
using Catalog.API.Extensions;

namespace Catalog.API.Services.Caches;

public interface ISubCategoryCachedService
{
    Task<List<SubCategoryCachedModel>?> QueryCachedSubCategoriesAsync(Func<SubCategoryCachedModel, bool> predicate, CancellationToken cancellationToken = default );
    Task<SubCategoryCachedModel?> GetSubCategoryCachedBySearchAsync(string search, PropertyName propertyName, CancellationToken cancellationToken = default);
    Task<List<SubCategoryCachedModel>?> GetCachedSubCategoriesAsync (CancellationToken cancellationToken = default );
    Task<SubCategoryCachedModel?> GetCachedSubCategoriesByIdAsync (string id, CancellationToken cancellationToken= default );
    Task<SubCategoryCachedModel?> GetCachedSubCategoriesByNameAsync(string name, CancellationToken cancellationToken = default);
}

public class SubCategoryCachedService : CommonCacheService, ISubCategoryCachedService
{
    private readonly SemaphoreSlim semaphore = new(1, 1);
    
    private const string _subCategoryKey = "SubCategory";
    
    private readonly IRepository<Entities.SubCategory> _subCategoriesRepository;

    public SubCategoryCachedService(IRedisDbFactory redisCache, IRepository<Entities.SubCategory> subCategoriesRepository)
        : base(_subCategoryKey, redisCache)
    {
        _subCategoriesRepository = subCategoriesRepository;
    }

    public async Task RefreshCachedSubCategoriesAsync (CancellationToken cancellationToken)
    {
        var supCategories = await _subCategoriesRepository.GetEntitiesAsync (cancellationToken);
        var subCategoryCached = supCategories.Select(x => x.ToCachedModel()).ToList();
        await SetAllItemsCacheAsync (subCategoryCached, cancellationToken);
    }

    public async Task<List<SubCategoryCachedModel>?> QueryCachedSubCategoriesAsync(Func<SubCategoryCachedModel, bool> predicate, CancellationToken cancellationToken)
    {
        List<SubCategoryCachedModel>? subCategories = await GetCachedSubCategoriesAsync(cancellationToken);

        if (subCategories is null) {
            return null;
        }

        return subCategories.Where(predicate).ToList();
    }

    public async Task<List<SubCategoryCachedModel>?> GetCachedSubCategoriesAsync(CancellationToken cancellationToken)
    {
        List<SubCategoryCachedModel>? subCategories = await GetAllItemAsync<SubCategoryCachedModel>(cancellationToken);

        if (subCategories is not null && subCategories.Count != 0) {
            return subCategories;
        }

        await semaphore.WaitAsync(cancellationToken);

        try
        {
            subCategories = await GetAllItemAsync<SubCategoryCachedModel>(cancellationToken);
            
            if (subCategories is not null && subCategories.Count != 0)
            {
                return subCategories;
            }

            subCategories = [];

            var subCategoryEntities = await _subCategoriesRepository.GetEntitiesAsync(cancellationToken);
            var subCategoryCached = subCategoryEntities.Select(x => x.ToCachedModel()).ToList();

            await SetAllItemsCacheAsync(subCategoryCached, cancellationToken);
        }
        finally
        {
            semaphore.Release();
        }

        return subCategories;
    }
    //hàm tìm theo 3 điều kiện
    // gọi tới 3 thằng trong common
    public async Task<SubCategoryCachedModel?> GetSubCategoryCachedBySearchAsync(string search, PropertyName propertyName, CancellationToken cancellationToken)
    {
        var data = propertyName switch
        {
            PropertyName.Id => await GetItemCachedByIdAsync<SubCategoryCachedModel>(search, cancellationToken),
            PropertyName.Name => await GetItemCacheByNameAsync<SubCategoryCachedModel>(search, cancellationToken),
            _ => null,
        };

        if (data  is not null)
        {
            return data;
        }

        await semaphore.WaitAsync(cancellationToken);
        try
        {
            var entity = propertyName switch
            {
                PropertyName.Id => await _subCategoriesRepository.GetEntityFirstOrDefaultAsync(x => x.Id == search, cancellationToken),
                PropertyName.Name => await _subCategoriesRepository.GetEntityFirstOrDefaultAsync(x => x.Name == search, cancellationToken),
                _ => null,
            };

            if (entity is null) 
            {
                return null;
            }

            data = await SetItemCacheAsync(entity.ToCachedModel(), cancellationToken);
        }
        finally 
        { 
            semaphore.Release(); 
        }

        return data;

    }
    

    public async Task<SubCategoryCachedModel?> GetCachedSubCategoriesByIdAsync(string id, CancellationToken cancellationToken)
    {
        var subCategory = await GetItemCachedByIdAsync<SubCategoryCachedModel>(id, cancellationToken);

        if(subCategory is null || subCategory.HasChange)
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                subCategory = await GetItemCachedByIdAsync<SubCategoryCachedModel>(id, cancellationToken);

                if (subCategory is not null )
                {
                    return subCategory;
                }

                var entity = await _subCategoriesRepository.GetEntityFirstOrDefaultAsync(x => x.Id == id, cancellationToken);

                if(entity is null)
                {
                    return null;
                }

                subCategory = await SetItemCacheAsync(entity.ToCachedModel(), cancellationToken);
            }
            finally 
            { 
                semaphore.Release(); 
            }
        }
        return subCategory;
    }

    public async Task<SubCategoryCachedModel?> GetCachedSubCategoriesByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var subCategory = await GetItemCacheByNameAsync<SubCategoryCachedModel>(name, cancellationToken);

        if (subCategory is null || subCategory.HasChange)
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                subCategory = await GetItemCacheByNameAsync<SubCategoryCachedModel>(name, cancellationToken);

                if (subCategory is not null)
                {
                    return subCategory;
                }

                var entity = await _subCategoriesRepository.GetEntityFirstOrDefaultAsync(x => x.Name == name, cancellationToken);

                if (entity is null)
                {
                    return null;
                }

                subCategory = await SetItemCacheAsync(entity.ToCachedModel(), cancellationToken);
            }
            finally
            {
                semaphore.Release();
            }
        }
        return subCategory;
    }
}
