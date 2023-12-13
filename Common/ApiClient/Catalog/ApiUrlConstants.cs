namespace ApiClient.Catalog;

public static class ApiUrlConstants
{
    public const string GetProducts = "/Catalog";

    public const string GetProductById = "/Catalog/GetProductById/{id}";

    public const string GetProductByCategory = "/Catalog/GetProductByCategory/{category}";

    public const string CreateProduct = "/Catalog";

    public const string UpdateProduct = "/Catalog/UpdateProduct";

    public const string DeleteProduct = "/Catalog/DeleteProductById/{id}";

    public const string GetCategories = "/Category/GetCategories";

    public const string GetCategoryById = "/Category/GetCategoryById/{id}";

    public const string GetCategoryByName = "/Category/GetCategoryById/{name}";

    public const string CreateCategory = "/Category/CreateCategory";

    public const string UpdateCategory = "/Catalog/UpdateCategory";

    public const string DeleteCategory = "/Catalog/DeleteCategory/{id}";
}