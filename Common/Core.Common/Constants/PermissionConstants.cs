namespace Core.Common.Constants;

public static class PermissionConstants
{
    public static class Application
    {
        public static class Name
        {
            public const string CatalogApi = "CatalogApi";
            public const string DiscountApi = "CatalogApi";
            public const string DiscountGrpc = "CatalogApi";
            public const string BasketApi = "CatalogApi";
            public const string OrderingApi = "CatalogApi";
        }
    }

    public static class Feature
    {
        public static class CatalogApi
        {
            public const string GetAllProducts = "GetAllProducts";
            public const string GetProductById = "GetProductById";
            public const string CreateProduct = "CreateProduct";
            public const string UpdateProduct = "UpdateProduct";
            public const string DeleteProduct = "DeleteProduct";
        }
    }
}