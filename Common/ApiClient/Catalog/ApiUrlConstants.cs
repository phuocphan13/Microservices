namespace ApiClient.Catalog;

public static class ApiUrlConstants
{
    public const string GetProducts = "/Catalog";

    public const string GetProductById = "/Catalog/GetProductById/{id}";

    public const string GetProductByCategory = "/Catalog/GetProductByCategory/{category}";

    public const string CreateProduct = "/Catalog/CreateProduct";

    public const string UpdateProduct = "/Catalog/UpdateProduct";

    public const string DeleteProduct = "/Catalog/DeleteProductById/{id}";
}