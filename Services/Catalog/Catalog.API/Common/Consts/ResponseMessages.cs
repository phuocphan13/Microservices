namespace Catalog.API.Common.Consts;

public static class ResponseMessages
{
    public static class Product
    {
        public static string PropertyNotExisted(string propertyName, string? name) => $"{propertyName} with Name {name} is not existed";

        public const string CreatFailure = "Creating Product is failed.";
    }
}