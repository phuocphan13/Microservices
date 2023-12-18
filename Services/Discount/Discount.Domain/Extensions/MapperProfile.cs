using AutoMapper;
using Discount.Domain.Entities;

namespace Discount.Domain.Extensions;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<DiscountHistory, DiscountVersion>().ReverseMap();
    }
}