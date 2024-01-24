namespace Platform.Constants;

public static class DatabaseConst
{
    public static class ConnectionSetting
    {
        public  static class MongoDB
        {
            public const string ConnectionString = "DatabaseSettings:MongoDb:ConnectionString";
            public const string DatabaseName = "DatabaseSettings:MongoDb:DatabaseName";
            public const string IsRebuildSchema = "DatabaseSettings:MongoDb:IsRebuildSchema";
            
            public static class CollectionName
            {
                public const string Product = "Products";
                public const string Category = "Categories";
                public const string SubCategory = "SubCategories";
            }
        }
        
        public static class Postgres
        {
            public const string IsRebuildSchema = "DatabaseSettings:Postgres:IsRebuildSchema";
            public const string ConnectionString = "DatabaseSettings:Postgres:ConnectionString";
        }
    }
}