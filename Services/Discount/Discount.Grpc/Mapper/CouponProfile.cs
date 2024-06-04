using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Discount.Models.Coupon;
using ApiClient.Discount.Models.Discount;
using AutoMapper;
using Coupon.Grpc.Protos;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Mapper;

public class CouponProfile : Profile
{
    public CouponProfile()
    {
        CreateMap<CouponDetail, CouponDetailModel>().ReverseMap();
        CreateMap<Coupon.Grpc.Protos.CreateCouponRequest, CreateCouponRequestBody>().ReverseMap();
        CreateMap<CreateCouponRequestBody,CouponDetailModel>().ReverseMap();
    }
}
