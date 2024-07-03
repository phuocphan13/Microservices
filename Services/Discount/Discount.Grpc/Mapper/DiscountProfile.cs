using ApiClient.Discount.Models.Coupon;
using ApiClient.Discount.Models.Discount;
using ApiClient.Discount.Models.Discount.AmountModel;
using AutoMapper;
using Coupon.Grpc.Protos;
using Discount.Grpc.Protos;
using ListCodeRequestModel = Discount.Grpc.Protos.ListCodeRequestModel;

namespace Discount.Grpc.Mapper;

public class DiscountProfile : Profile
{
    public DiscountProfile()
    {
        CreateMap<Domain.Entities.Discount, DiscountDetailModel>().ReverseMap();
        CreateMap<DiscountDetail, DiscountDetailModel>().ReverseMap();
        CreateMap<CouponDetail, CouponDetailModel>().ReverseMap();

        CreateMap<AmountAfterDiscountRequest, AmountDiscountRequestBody>().ReverseMap();
        CreateMap<CategoryRequestBody, ListCategoryModel>().ForMember(dest => dest.SubList, opt => opt.MapFrom(x => x.SubCategories)).ReverseMap();
        CreateMap<SubCategoryRequestBody, ListSubCategoryModel>().ForMember(dest => dest.ProdList, opt => opt.MapFrom(x => x.Products))
            .ForMember(dest => dest.SubType, opt => opt.MapFrom(x => x.Type))
            .ForMember(dest => dest.SubCatalogCode, opt => opt.MapFrom(x => x.CatalogCode)).ReverseMap();
        CreateMap<ProductRequestBody, ListProductModel>()
            .ForMember(dest => dest.ProdType, opt => opt.MapFrom(x => x.Type))
            .ForMember(dest => dest.ProdCatalogCode, opt => opt.MapFrom(x => x.CatalogCode)).ReverseMap();

        CreateMap<AmountDiscountResponseModel, DiscountResponse>().ReverseMap();

        CreateMap<DiscountResponse, DiscountDetail>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(x => x.Type))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(x => x.Amount)).ReverseMap();

        CreateMap<ListCodeRequestModel, ListCodeRequestBody>().ReverseMap();
    }
}
