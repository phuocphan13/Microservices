using ApiClient.Discount.Models.Coupon;
using AutoMapper;
using Coupon.Grpc.Protos;
using Discount.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTest.Common.Helpers;
using UnitTest.Common.Helpers.Grpc;

namespace Discount.Tests.Grpc;

public class CouponGrpcServiceTests
{
    [Test]
    public async Task GetCouponAsync_ExpectedResult()
    {
        string id = "1";
        string description = "Test Get Coupon";

        var request = new GetCouponRequest()
        {
            Id = id
        };

        var couponDetail = new CouponDetail()
        {
            Id = int.Parse(id),
            Description = description,
            Value = 20,
        };

        var couponModel = new CouponDetailModel
        {
            Id = int.Parse(id),
            Description = description,
            Value = 20.ToString(),
        };

        var couponService = new Mock<ICouponService>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<Discount.Grpc.Services.CouponService>>();

        couponService.Setup(x => x.GetCouponAsync(It.IsAny<string>())).ReturnsAsync(couponDetail);
        mapper.ConfigMapper<CouponDetail, CouponDetailModel>(couponModel);

        var service = new Discount.Grpc.Services.CouponService((ILogger<Discount.Grpc.Services.CouponService>)couponService.Object, (IMapper)logger.Object, (ICouponService)mapper.Object);
        var result = await service.GetCoupon(request, TestServerCallContextHelpers.Create());

        Assert.Multiple(() =>
        {
            Assert.That(id, Is.EqualTo(result.Id.ToString()));
            Assert.That(description, Is.EqualTo(result.Description));
        });
    }

    [Test]
    public async Task CreateCouponAsync_ExpectedResult()
    {
        string id = "1";
        string description = "Test Create Coupon";
        string name = "Test";

        var couponDetail = new CouponDetail()
        {
            Id = int.Parse(id),
            Description = description,
            Name = name,
            Value = 20
        };

        var couponModel = new CouponDetailModel()
        {
            Id = int.Parse(id),
            Description = description,
            Name = name,
            Value = 20.ToString(),
            Type = (CouponType)1
        };
}
}