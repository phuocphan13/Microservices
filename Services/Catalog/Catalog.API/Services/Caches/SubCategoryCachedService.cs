

using Catalog.API.Entities;
using Catalog.API.Repositories;
using Platform.Database.Redis;
using Catalog.API.Models;
using Catalog.API.Extensions;
using System.Collections.Generic;

namespace Catalog.API.Services.Caches;

public interface ISubCategoryCachedService
{
    Task<List<SubCategoryCachedModel>?> QueryCachedSubCategoriesAsync(Func<SubCategoryCachedModel, bool> predicate, CancellationToken cancellationToken = default );
    Task<List<SubCategoryCachedModel>?> GetCachedSubCategoriesAsync (CancellationToken cancellationToken= default );
    Task<SubCategoryCachedModel>? GetCachedSubCategoriesByIdAsync (string id, CancellationToken cancellationToken= default );
}

public class SubCategoryCachedService : CommonCacheService, ISubCategoryCachedService
{
    private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    private const string _subCategoryKey = "SubCategory";
    private readonly IRepository<SubCategory> _subCategoriesRepository;

    public SubCategoryCachedService(IRedisDbFactory redisCache, IRepository<SubCategory> subCategoriesRepository)
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

        if (subCategories is not null && subCategories.Any()) {
            return subCategories;
        }


        await semaphore.WaitAsync(cancellationToken);

        try
        {
            subCategories = await GetAllItemAsync<SubCategoryCachedModel>(cancellationToken);
            if (subCategories is not null && subCategories.Any())
            {
                return subCategories;
            }

            subCategories = new();

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

    public async Task<SubCategoryCachedModel>? GetCachedSubCategoriesByIdAsync(string id, CancellationToken cancellationToken)
    {
        var subCategory = await GetItemCachedByIdAsync<SubCategoryCachedModel>(id, cancellationToken);

        if(subCategory is null|| subCategory.HasChange)
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                subCategory = await GetItemCachedByIdAsync<SubCategoryCachedModel>(id, cancellationToken);

                if (subCategory is null )
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
}
