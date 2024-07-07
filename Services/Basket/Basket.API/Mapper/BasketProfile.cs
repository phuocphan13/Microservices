using ApiClient.Basket.Events.CheckoutEvents;
using ApiClient.Basket.Models;
using AutoMapper;
using Basket.API.Entitites;

namespace Basket.API.Mapper
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutMessage>().ReverseMap();
            CreateMap<BasketDetail, BasketCheckoutMessage>().ReverseMap();
        }
    }
}
