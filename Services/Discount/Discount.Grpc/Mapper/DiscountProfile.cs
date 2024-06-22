﻿using ApiClient.Discount.Models.Coupon;
using ApiClient.Discount.Models.Discount;
using ApiClient.Discount.Models.Discount.AmountModel;
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
        CreateMap<AmountAfterDiscountRequest, AmountDiscountRequestBody>().ReverseMap();
        CreateMap<CategoryRequestBody, ListCategoryModel>().ReverseMap();
        CreateMap<SubCategoryRequestBody, ListSubCategoryModel>().ReverseMap();
        CreateMap<ProductRequestBody, ListProductModel>().ReverseMap();
        CreateMap<AmountDiscountResponseModel, DiscountResponse>().ReverseMap();
        CreateMap<AmountDiscountResponseModel, Domain.Entities.Discount>().ReverseMap();
    }
}
