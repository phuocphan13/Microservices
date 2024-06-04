using ApiClient.Common;
using Microsoft.Extensions.Configuration;
using Platform.Common.Session;

namespace ApiClient.Basket;

public interface IBasketApiClient
{
    
}

public class BasketApiClient : CommonApiClient, IBasketApiClient
{
    protected BasketApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ISessionState sessionState) 
        : base(httpClientFactory, configuration, sessionState)
    {
    }
}