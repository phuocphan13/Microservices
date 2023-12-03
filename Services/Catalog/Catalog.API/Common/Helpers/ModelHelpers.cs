namespace Catalog.API.Common.Helpers;

public static class ModelHelpers
{
    public static string GenerateGuid()
    {
        return Guid.NewGuid().ToString().Replace("-", "");
    }

    public static string GenerateId()
    {
        return Guid.NewGuid().ToString().Replace("-", "")[..24];
    }
}