using ApiClient.Basket.Events.CheckoutEvents;
using AutoMapper;
using EventBus.Messages.Entities;

namespace EventBus.Messages.Mapper;

public sealed class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<BasketCheckoutMessage, Basket>();
    }
}