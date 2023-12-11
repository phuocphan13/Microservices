namespace Catalog.API.Common.Consts;

public static class ResponseMessages
{
    public static class Product
    {
        public static string PropertyNotExisted(string propertyName, string? name) => $"{propertyName} with Name {name} is not existed";

        public static string ProductExisted(string? name) => $"Product with Name '{name}' is existed.";

        public const string CreatFailure = "Creating Product is failed.";

        public const string NotFound = "Not Found.";

        public const string UpdateFailed = "Update Has Failed.";
    }

    public static class Category
    {
        public static string PropertyNotExisted(string propertyName, string? name) => $"{propertyName} with Name {name} is not existed";

        public static string CategoryExisted(string? name) => $"Category with Name '{name}' is existed.";

        public const string CreateFailed = "Created Category Has Failed.";

        public const string NotFound = "Not Found.";

        public const string UpdateFailed = "Update Has Failed.";

    }
}