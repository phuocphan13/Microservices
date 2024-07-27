using Platform.ApiBuilder;
using Discount.Grpc.Protos;
using ApiClient.Discount.Models.Discount.ActiveModel;
using AngularClient.Extensions;

namespace AngularClient.Services;

public interface IDiscountService
{
    Task<InactiveResponseModel> InactiveDiscountAsync ( int id, CancellationToken cancellationToken );
}

public class DiscountService : IDiscountService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoApiClient;

    public DiscountService (DiscountProtoService.DiscountProtoServiceClient discountProtoApiClient)
    {
        _discountProtoApiClient = discountProtoApiClient;
    }

    public Task<InactiveResponseModel> InactiveDiscountAsync (int id, CancellationToken cancellationToken )
    {
        var request = new InactiveRequestBody() { 
            Id = id,
        };

        var result =  _discountProtoApiClient.InactiveDiscount(request.ToInactiveRequestBody());

        return null;
    }
}
