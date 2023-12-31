namespace Catalog.API.Common.Extensions;

public static class DatabaseExtensions
{
    public static string GetCollectionName(Type documentType)
    {
        return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                typeof(BsonCollectionAttribute),
                true)
            .First()).CollectionName;
    }
}