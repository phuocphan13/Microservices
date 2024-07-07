using ApiClient.Basket.Events.CheckoutEvents;
using AutoMapper;
using Ordering.Application.Features.Commands.CheckoutOrder;

namespace Ordering.API.Mapping;

public class OrderingProfile : Profile
{
    public OrderingProfile()
    {
        CreateMap<CheckoutOrderCommand, BasketCheckoutMessage>().ReverseMap();
    }
}