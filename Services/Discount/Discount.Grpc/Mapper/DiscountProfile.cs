﻿using ApiClient.Discount.Models.Coupon;
using ApiClient.Discount.Models.Discount;
using AutoMapper;
using Coupon.Grpc.Protos;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Mapper;

public class DiscountProfile : Profile
{
    public DiscountProfile()
    {
        CreateMap<Domain.Entities.Discount, DiscountDetailModel>().ReverseMap();
        CreateMap<DiscountDetail, DiscountDetailModel>().ReverseMap();
        
        CreateMap<CouponDetail, CouponDetailModel>().ReverseMap();
    }
}
