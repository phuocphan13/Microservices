using AutoMapper;
using Discount.Domain.Entities;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Mapper;

public class DiscountProfile : Profile
{
    public DiscountProfile()
    {
        CreateMap<Coupon, CouponDetailModel>().ReverseMap();
    }
}
