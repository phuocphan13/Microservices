namespace ApiClient.Catalog;

public static class ApiUrlConstants
{
    public const string GetProducts = "/Product";

    public const string GetProductById = "/Product/GetProductById/{id}";

    public const string GetProductByCategory = "/Product/GetProductByCategory/{category}";

    public const string CreateProduct = "/Product";

    public const string UpdateProduct = "/Product/UpdateProduct";

    public const string DeleteProduct = "/Product/DeleteProductById/{id}";

    public const string GetCategories = "/Category/GetCategories";

    public const string GetCategoryById = "/Category/GetCategoryById/{id}";

    public const string GetCategoryByName = "/Category/GetCategoryByName/{name}";

    public const string CreateCategory = "/Category/CreateCategory";

    public const string UpdateCategory = "/Category/UpdateCategory";

    public const string DeleteCategory = "/Category/DeleteCategory/{id}";

    //Url for SubCategory

    public const string GetSubCategories = "/SubCategory/GetSubCategories";

    public const string GetSubCategoryById = "/SubCategory/GetSubCategoryById/{id}";

    public const string GetSubCategoryByName = "/SubCategory/GetSubCategoryByName/{name}";

    public const string GetSubCategoryByCategoryId = "/SubCategory/GetSubCategoriesByCategoryId/{categoryId}";

    public const string CreateSubCategory = "/SubCategory/CreateSubCategory";

    public const string UpdateSubCategory = "/SubCategory/UpdateSubCategory";

    public const string DeleteSubCategory = "/SubCategory/DeleteSubCategory/{id}";
    
    //Validation
    public const string ValidateCatalogCode = "/Validation/ValidateCatalogCode";
}