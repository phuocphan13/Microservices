using ApiClient.Catalog.Category.Models;
using ApiClient.Common;
using ApiClient.Discount.Models.Coupon;
using Microsoft.Extensions.Configuration;
using Platform.ApiBuilder;
using Platform.Common.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Discount.Coupon
{
    public interface ICouponApiClient
    {
        Task<ApiCollectionResult<CouponSummary>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    }
    public class CouponApiClient : CommonApiClient, ICouponApiClient
    {
        public CouponApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ISessionState sessionState)
    : base(httpClientFactory, configuration, sessionState)
        {
        }
        public async Task<ApiCollectionResult<CouponSummary>> GetCategoriesAsync(CancellationToken cancellationToken)
        {
            var url = $"{GetBaseUrl()}{ApiUrlConstants.GetAllCoupons}";

            var result = await GetCollectionAsync<CouponSummary>(url, cancellationToken);

            return result;
        }
    }
}
