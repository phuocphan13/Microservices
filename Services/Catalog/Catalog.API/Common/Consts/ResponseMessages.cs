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

        public static string CategoryExisted(string? name, string? code) => $"Category with Name '{name}' or Code '{code} is existed.";

        public static string CategoryCodeExisted(string? code) => $"Category with Code '{code}' is existed.";

        public const string CreateFailed = "Created Category Has Failed.";

        public const string NotFound = "Not Found.";

        public const string UpdateFailed = "Update Has Failed.";

        public const string DeleteFailed = "Delete Has Failed.";
    }

    public static class SubCategory
    {
        public static string NotFound = "SubCategory not found.";
        public static string SubCategoryExisted => $"SubCategory is existed.";
        public static string CategoryIdNotFound(string? CategoryId) => $"Category's ID '{CategoryId}' does not existed.";

        public const string CreateSubCategoryFailed = "Created SubCategory Has Failed.";

        public const string UpdateSubCategoryFailed = "Updated SubCategory Has Failed.";

        public const string SubCategoryCodeExisted = "SubCategory Name ";
    }

    public static class Delete
    {
        public static string NotFound = "Product not found.";
        public static string DeleteFailed = "Delete failed.";
        public static string DeleteSuccess = "Delete successfully.";
    }
}