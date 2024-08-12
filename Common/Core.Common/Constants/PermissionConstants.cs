namespace Core.Common.Constants;

public static class PermissionConstants
{
    public static class Application
    {
        public static class Name
        {
            public const string CatalogApi = "CatalogApi";
            public const string DiscountApi = "DiscountApi";
            public const string DiscountGrpc = "DiscountGrpc";
            public const string BasketApi = "BasketApi";
            public const string OrderingApi = "OrderingApi";
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