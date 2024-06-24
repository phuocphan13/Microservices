using ApiClient.Basket.Events.CheckoutEvents;
using ApiClient.Basket.Models;
using AutoMapper;
using Ordering.Application.Features.Commands.CheckoutOrder;
using Ordering.Application.Features.Commands.UpdateOrder;
using Ordering.Application.Features.Queries.GetOrderList;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrdersVm>().ReverseMap();
        
        CreateMap<Order, UpdateOrderCommand>().ReverseMap();

        CheckoutOrderMapping();
    }

    private void CheckoutOrderMapping()
    {
        CreateMap<DiscountItemSummary, DiscountItem>().ReverseMap();
        CreateMap<CouponItemSummary, CouponItem>().ReverseMap();
        CreateMap<BasketItemSummary, OrderItem>().ReverseMap();

        CreateMap<Order, CheckoutOrderCommand>().ReverseMap()
            .ForMember(dest => dest.Coupons, opt => opt.MapFrom(src => src.CouponItems))
            .ForMember(dest => dest.Discounts, opt => opt.MapFrom(src => src.DiscountItems))
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items));

        CreateMap<BasketCheckoutMessage, CheckoutOrderCommand>().ReverseMap();
    }
}
