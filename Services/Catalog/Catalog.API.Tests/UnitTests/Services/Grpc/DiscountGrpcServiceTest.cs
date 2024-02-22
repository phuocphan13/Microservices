using Catalog.API.Services.Grpc;
using Discount.Grpc.Protos;
using UnitTest.Common.Helpers;

namespace Catalog.API.Tests.UnitTests.Services.Grpc;

public class DiscountGrpcServiceTest
{
    [Fact]
    public void Constructor_NullParams_ThrowException()
    {
        DiscountProtoService.DiscountProtoServiceClient discountGrpcService = null!;
        Assert.Throws<ArgumentNullException>(
            nameof(discountGrpcService),
            () =>
            {
                _ = new DiscountGrpcService(discountGrpcService);
            });
    }

    #region GetDiscount
    [Fact]
    public async Task GetDiscount_ValidParams_ExpectedResult()
    {
        string productName = CommonHelpers.GenerateRandomString();
        var response = new DiscountDetailModel { CatalogName = productName };

        var call = GrpcHelpers.BuildAsyncUnaryCall(response);

        var discountGrpcService = new Mock<DiscountProtoService.DiscountProtoServiceClient>();

        discountGrpcService.Setup(x => x.GetDiscountAsync(It.IsAny<GetDiscountRequest?>(), null, null, default)).Returns(call);

        var service = new DiscountGrpcService(discountGrpcService.Object);

        var detail = await service.GetDiscount(productName);

        Assert.NotNull(detail);
        Assert.Equal(detail.CatalogCode, productName);
    }
    #endregion

    #region GetDiscountByCatalogCode
    [Fact]
    public async Task GetDiscountByCatalogCode_ValidParams_ExpectedResult()
    {
        string productName = CommonHelpers.GenerateRandomString();
        var response = new DiscountDetailModel { CatalogName = productName, Type = Discount.Grpc.Protos.CatalogType.Product };

        var call = GrpcHelpers.BuildAsyncUnaryCall(response);

        var discountGrpcService = new Mock<DiscountProtoService.DiscountProtoServiceClient>();

        discountGrpcService.Setup(x => x.GetDiscountByCatalogCodeAsync(It.IsAny<GetDiscountByCatalogCodeRequest?>(), null, null, default)).Returns(call);

        var service = new DiscountGrpcService(discountGrpcService.Object);

        var detail = await service.GetDiscountByCatalogCode(DiscountEnum.Product, productName);

        Assert.NotNull(detail);
        Assert.Equal(detail.CatalogCode, productName);
    }
    #endregion

    #region GetListDiscountsByCatalogCodeAsync
    [Fact]
    public async Task GetListDiscountsByCatalogCodeAsync_ValidParams_WithNullResponse_ExpectedResult()
    {
        string productName = CommonHelpers.GenerateRandomString();

        var call = GrpcHelpers.BuildAsyncUnaryCall<ListDetailsModel>(null!);

        var discountGrpcService = new Mock<DiscountProtoService.DiscountProtoServiceClient>();

        discountGrpcService.Setup(x => x.GetListDiscountsAsync(It.IsAny<GetListDiscountRequest?>(), null, null, default)).Returns(call);

        var service = new DiscountGrpcService(discountGrpcService.Object);

        var detail = await service.GetListDiscountsByCatalogCodeAsync(DiscountEnum.Product, new List<string> { productName });

        Assert.Null(detail);
    }
    
    [Fact]
    public async Task GetListDiscountsByCatalogCodeAsync_ValidParams_WithNotAnyResponse_ExpectedResult()
    {
        string productName = CommonHelpers.GenerateRandomString();
        var response = new ListDetailsModel();

        var call = GrpcHelpers.BuildAsyncUnaryCall(response);

        var discountGrpcService = new Mock<DiscountProtoService.DiscountProtoServiceClient>();

        discountGrpcService.Setup(x => x.GetListDiscountsAsync(It.IsAny<GetListDiscountRequest?>(), null, null, default)).Returns(call);

        var service = new DiscountGrpcService(discountGrpcService.Object);

        var detail = await service.GetListDiscountsByCatalogCodeAsync(DiscountEnum.Product, new List<string> { productName });

        Assert.Null(detail);
    }
    
    [Fact]
    public async Task GetListDiscountsByCatalogCodeAsync_ValidParams_ExpectedResult()
    {
        string productName = CommonHelpers.GenerateRandomString();
        var response = new ListDetailsModel
        {
            Items =
            {
                new DiscountDetailModel { CatalogName = productName, Type = Discount.Grpc.Protos.CatalogType.Product },
                new DiscountDetailModel { CatalogName = productName, Type = Discount.Grpc.Protos.CatalogType.Product },
                new DiscountDetailModel { CatalogName = productName, Type = Discount.Grpc.Protos.CatalogType.Product },
            }
        };

        var call = GrpcHelpers.BuildAsyncUnaryCall(response);

        var discountGrpcService = new Mock<DiscountProtoService.DiscountProtoServiceClient>();

        discountGrpcService.Setup(x => x.GetListDiscountsAsync(It.IsAny<GetListDiscountRequest?>(), null, null, default)).Returns(call);

        var service = new DiscountGrpcService(discountGrpcService.Object);

        var detail = await service.GetListDiscountsByCatalogCodeAsync(DiscountEnum.Product, new List<string> { productName });

        Assert.NotNull(detail);
        Assert.Equal(3, detail.Count);
    }
    #endregion
}