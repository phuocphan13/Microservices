using ApiClient.Basket.Events.CheckoutEvents;
using ApiClient.Catalog.Product.Events;
using AutoMapper;
using EventBus.Messages.Entities;

namespace EventBus.Messages.Mapper;

public sealed class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<BasketCheckoutMessage, Order>()
            .ForMember(m => m.ReceiptNumber, opt => opt.MapFrom(src => src.BasketKey)).ReverseMap();
        CreateMap<ProductBalanceUpdateMessage, Order>()
            .ForMember(m => m.ReceiptNumber, opt => opt.MapFrom(src => src.ReceiptNumber)).ReverseMap();
    }
}