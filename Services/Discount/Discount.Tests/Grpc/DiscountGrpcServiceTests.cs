using ApiClient.Discount.Models.Discount;
using ApiClient.Discount.Models.Discount.AmountModel;
using AutoMapper;
using Discount.Domain.Services;
using Discount.Grpc.Protos;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTest.Common.Helpers;
using UnitTest.Common.Helpers.Grpc;
using GrpcServices = Discount.Grpc.GrpcServices;

namespace Discount.Tests.Grpc;

public class DiscountGrpcServiceTests
{
    [Test]
    public async Task GetDiscountAsync_ExpectedResult()
    {
        string id = "1";
        string description = "Test Discount";

        var request = new GetDiscountRequest()
        {
            Id = id
        };

        var discountDetail = new DiscountDetail()
        {
            Id = int.Parse(id),
            Amount = 10,
            Description = description,
        };
        
        var discountModel = new DiscountDetailModel
        {
            Id = int.Parse(id),
            Amount = 10,
            Description = description,
        };

        var discountService = new Mock<IDiscountService>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<GrpcServices.DiscountService>>();

        discountService.Setup(x => x.GetDiscountAsync(It.IsAny<string>())).ReturnsAsync(discountDetail);
        mapper.ConfigMapper<DiscountDetail, DiscountDetailModel>(discountModel);

        var service = new GrpcServices.DiscountService(discountService.Object, logger.Object, mapper.Object);
        var result = await service.GetDiscount(request, TestServerCallContextHelpers.Create());
        
        Assert.Multiple(() =>
        {
            Assert.That(id, Is.EqualTo(result.Id.ToString()));
            Assert.That(description, Is.EqualTo(result.Description));
        });
    }

    [Test]
    public async Task CreateDiscountAsync_ExpectedResult()
    {
        string id = "1";
        string description = "Test Discount";
        string code = CommonHelpers.GenerateRandomString();
        
        var discountDetail = new DiscountDetail()
        {
            Id = int.Parse(id),
            Amount = 10,
            Description = description,
        };

        var discountModel = new DiscountDetailModel()
        {
            Id = int.Parse(id),
            Amount = 10,
            Description = description,
        };

        var request = new CreateDiscountRequest()
        {
            Amount = 10,
            Description = description,
            Code = code,
        };

        var requestBody = new CreateDiscountRequestBody()
        {
            Amount = 10,
            Description = description,
            CatalogCode = code
        };
        
        var discountService = new Mock<IDiscountService>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<GrpcServices.DiscountService>>();

        discountService.Setup(x => x.CreateDiscountAsync(It.IsAny<CreateDiscountRequestBody>(), It.IsAny<CancellationToken>())).ReturnsAsync(discountDetail);
        mapper.ConfigMapper<CreateDiscountRequest, CreateDiscountRequestBody>(requestBody);
        mapper.ConfigMapper<DiscountDetail, DiscountDetailModel>(discountModel);
        
        var service = new GrpcServices.DiscountService(discountService.Object, logger.Object, mapper.Object);
        var result = await service.CreateDiscount(request, TestServerCallContextHelpers.Create());

        Assert.Multiple(() =>
        {
            Assert.That(id, Is.EqualTo(result.Id.ToString()));
            Assert.That(description, Is.EqualTo(result.Description));
        });
    }

    [Test]
    public async Task TotalDiscountAmount_ExpectedResult()
    {
        string code = "Pro1.Sub1.Cate1";
        var requestBody = new ListCodeRequest();
        requestBody.Codes.Add(code);

        var totalAmountModel = new List<TotalAmountModel>
        {
            new()
            {
                CatalogCode = "Pro1",
                Amount = 30,
            }
        };
        
        var discountService = new Mock<IDiscountService>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<GrpcServices.DiscountService>>();

        discountService.Setup(x => x.TotalDiscountAmountAsync(It.IsAny<List<CombinationCodeRequestBody>>())).ReturnsAsync(totalAmountModel);


        var service = new GrpcServices.DiscountService(discountService.Object, logger.Object, mapper.Object);
        var result = await service.TotalDiscountAmount(requestBody, TestServerCallContextHelpers.Create());

        var totalAmount = result.AmountDiscountResponse.FirstOrDefault(x => x.CatalogCode == "Pro1");
        
        Assert.Multiple(() =>
        {
            Assert.That(totalAmount, Is.Not.Null);
            Assert.That(int.Parse(totalAmount!.Amount), Is.EqualTo(30));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AmountDiscountResponse, Has.Count.EqualTo(1));
        });
    }
}