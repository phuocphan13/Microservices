﻿using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Common;
using Catalog.API.Common.Consts;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Repositories;

namespace Catalog.API.Services;

public interface ICategoryService
{
    Task<ApiDataResult<List<CategorySummary>>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<ApiDataResult<CategorySummary>> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ApiDataResult<CategorySummary>> GetCategoryByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<ApiDataResult<CategorySummary>> GetCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<ApiDataResult<CategorySummary>> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ApiStatusResult> DeleteCategoryAsync(string id, CancellationToken cancellationToken = default);
}

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;

    public CategoryService(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ApiDataResult<List<CategorySummary>>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        var categoryList = new ApiDataResult<List<CategorySummary>>
        {
            Data = new List<CategorySummary>()
        };

        var entities = await _categoryRepository.GetEntitiesAsync(cancellationToken);

        if (entities is null)
        {
            categoryList.Message = ResponseMessages.Category.NotFound;
            return categoryList;
        }

        foreach (var entity in entities)
        {
            categoryList.Data.Add(entity.ToSummary());
        }

        return categoryList;
    }

    public async Task<ApiDataResult<CategorySummary>> GetCategoryByNameAsync(string name, CancellationToken cancellationToken)
    {
        var result = new ApiDataResult<CategorySummary>();

        var entity = await _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.Name == name, cancellationToken);

        if (entity is null)
        {
            result.Message = ResponseMessages.Category.NotFound;
            return result;
        }

        result.Data = entity.ToSummary();

        return result;
    }

    public async Task<ApiDataResult<CategorySummary>> GetCategoryByIdAsync(string id, CancellationToken cancellationToken)
    {
        var result = new ApiDataResult<CategorySummary>();

        var entity = await _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            result.Message = ResponseMessages.Category.NotFound;
            return result;
        }

        result.Data = entity.ToSummary();

        return result;
    }

    public async Task<ApiDataResult<CategorySummary>> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var apiDataResult = new ApiDataResult<CategorySummary>();

        var isExisted = await _categoryRepository.AnyAsync(x => x.Name == requestBody.Name || x.CategoryCode == requestBody.CategoryCode, cancellationToken);

        if (isExisted)
        {
            apiDataResult.Message = ResponseMessages.Category.CategoryExisted(requestBody.Name, requestBody.CategoryCode);
            return apiDataResult;
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

        var isExisted = await _categoryRepository.AnyAsync(x => (x.CategoryCode == requestBody.CategoryCode || x.Name == requestBody.Name) && x.Id != requestBody.Id, cancellationToken);

        if (isExisted)
        {
            apiDataResult.Message = ResponseMessages.Category.CategoryExisted(requestBody.Name, requestBody.CategoryCode);
            return apiDataResult;
        }

        category.ToUpdateCategory(requestBody);

        var updateItem = await _categoryRepository.UpdateEntityAsync(category, cancellationToken);

        if (!updateItem)
        {
            apiDataResult.Message = ResponseMessages.Category.UpdateFailed;
            return apiDataResult;
        }

        apiDataResult.Data = category.ToSummary();
        
        return apiDataResult;
    }

    public async Task<ApiStatusResult> DeleteCategoryAsync(string id, CancellationToken cancellationToken)
    {
        var apiDataResult = new ApiStatusResult();
        var isExisted = await _categoryRepository.AnyAsync(x => x.Id == id, cancellationToken);

        if (!isExisted)
        {
            apiDataResult.Message = ResponseMessages.Category.NotFound;
            return apiDataResult;
        }

        var result = await _categoryRepository.DeleteEntityAsync(id, cancellationToken);

        if (!result)
        {
            apiDataResult.Message = ResponseMessages.Category.UpdateFailed;
            return apiDataResult;
        }

        return apiDataResult;
    }
}