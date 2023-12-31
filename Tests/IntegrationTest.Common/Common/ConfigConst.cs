using Core.Common.Api;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTest.Common.Common;

public static class ConfigConst
{
    public static WebApplicationFactoryClientOptions ConfigFactoryClientOptions(string ApiName)
    {
        string url = ApiName switch
        {
            ConfigurationConst.CatalogApiName => ConfigurationConst.Development.CatalogApiUrl,
            _ => string.Empty
        };

        return new WebApplicationFactoryClientOptions()
        {
            BaseAddress = new Uri(url)
        };
    }
}