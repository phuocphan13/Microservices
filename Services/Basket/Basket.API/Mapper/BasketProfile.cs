using ApiClient.Basket.Events.CheckoutEvents;
using ApiClient.Basket.Models;
using AutoMapper;
using Basket.API.Entitites;

namespace Basket.API.Mapper;

public class BasketProfile : Profile
{
    public BasketProfile()
    {
        CreateMap<BasketCheckout, BasketCheckoutMessage>().ReverseMap();
        CreateMap<BasketDetail, BasketCheckoutMessage>().ReverseMap();
        CreateMap<Basket.API.Entitites.Basket, BasketCheckoutMessage>().ReverseMap()
            .ForMember(x => x.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(x => x.Discounts, opt => opt.MapFrom(src => src.DiscountItems))
            .ForMember(x => x.Coupons, opt => opt.MapFrom(src => src.CouponItems));
    }
}