using ApiClient.Discount.Models.Coupon;
using AutoMapper;
using Coupon.Grpc.Protos;

namespace Discount.Grpc.Mapper;

public class CouponProfile : Profile
{
    public CouponProfile()
    {
        CreateMap<CouponDetail, CouponDetailModel>().ReverseMap();
        CreateMap<Coupon.Grpc.Protos.CreateCouponRequest, CreateCouponRequestBody>().ReverseMap();
        CreateMap<CreateCouponRequestBody,CouponDetailModel>().ReverseMap();
        CreateMap<EditCouponRequest, UpdateCouponRequestBody>().ReverseMap();

        CreateMap<ApiClient.Discount.Models.Coupon.CouponSummary, Coupon.Grpc.Protos.CouponSummary>().ReverseMap();
        CreateMap<List<ApiClient.Discount.Models.Coupon.CouponSummary>, CouponListResponse>().ForMember(dest => dest.CouponList, opt => opt.MapFrom(src => src));

    }
}
