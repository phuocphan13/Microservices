using ApiClient.Catalog.SubCategory;
using ApiClient.Catalog.SubCategory.Models;
using ApiClient.Common;
using Microsoft.Extensions.Configuration;
using Platform.ApiBuilder;
using Platform.Common.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Discount.Discount;

public interface IDiscountApiClient
{
    Task<ApiStatusResult> InactiveDiscountAsync(string id, CancellationToken cancellationToken);
}

public class DiscountApiClient : CommonApiClient, IDiscountApiClient
{
    public DiscountApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ISessionState sessionState)
        : base(httpClientFactory, configuration, sessionState)
    {
    }

    public async Task<ApiStatusResult> InactiveDiscountAsync(int id,CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.InactiveDiscount}";

        var result = await PutAsync<ApiStatusResult>(url, id, cancellationToken);

        return result.Result;
    }
}
