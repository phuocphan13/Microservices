namespace Catalog.API.Common.Consts;

public static class DatabaseConst
{
    public static class CollectionName
    {
        public const string ConnectionString = "DatabaseSettings:ConnectionString";
        public const string DatabaseName = "DatabaseSettings:DatabaseName";
        public  const string IsRebuildSchema = "DatabaseSettings:IsRebuildSchema";
        
        public const string Product = "Products";
        public const string Category = "Categories";
        public const string SubCategory = "SubCategories";
    }
}