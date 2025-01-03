// using ApiClient.Discount.Models.Discount;
// using Catalog.API.Entities;
// using Catalog.API.Services.Grpc;
// using Catalog.API.Tests.Common;
// using Discount.Grpc.Protos;
// using UnitTest.Common.Helpers;
//
// namespace Catalog.API.Tests.UnitTests.Services.Grpc;
//
// public class DiscountGrpcServiceTest
// {
//     [Fact]
//     public void Constructor_NullParams_ThrowException()
//     {
//         DiscountProtoService.DiscountProtoServiceClient discountGrpcService = null!;
//         Assert.Throws<ArgumentNullException>(
//             nameof(discountGrpcService),
//             () =>
//             {
//                 _ = new DiscountGrpcService(discountGrpcService);
//             });
//     }
//
//     #region GetDiscount
//     [Fact]
//     public async Task GetDiscount_ValidParams_ExpectedResult()
//     {
//         string productName = CommonHelpers.GenerateRandomString();
//         var response = new DiscountDetailModel { CatalogName = productName };
//
//         var call = GrpcHelpers.BuildAsyncUnaryCall(response);
//
//         var discountGrpcService = new Mock<DiscountProtoService.DiscountProtoServiceClient>();
//
//         discountGrpcService.Setup(x => x.GetDiscountAsync(It.IsAny<GetDiscountRequest?>(), null, null, default)).Returns(call);
//
//         var service = new DiscountGrpcService(discountGrpcService.Object);
//
//         var detail = await service.GetDiscount(productName);
//
//         Assert.NotNull(detail);
//         Assert.Equal(detail.CatalogCode, productName);
//     }
//     #endregion
//
//     #region GetDiscountByCatalogCode
//     [Fact]
//     public async Task GetDiscountByCatalogCode_ValidParams_ExpectedResult()
//     {
//         string productName = CommonHelpers.GenerateRandomString();
//         var response = new DiscountDetailModel { CatalogName = productName, Type = Discount.Grpc.Protos.CatalogType.Product };
//
//         var call = GrpcHelpers.BuildAsyncUnaryCall(response);
//
//         var discountGrpcService = new Mock<DiscountProtoService.DiscountProtoServiceClient>();
//
//         discountGrpcService.Setup(x => x.GetDiscountByCatalogCodeAsync(It.IsAny<GetDiscountByCatalogCodeRequest?>(), null, null, default)).Returns(call);
//
//         var service = new DiscountGrpcService(discountGrpcService.Object);
//
//         var detail = await service.GetDiscountByCatalogCode(DiscountEnum.Product, productName);
//
//         Assert.NotNull(detail);
//         Assert.Equal(detail.CatalogCode, productName);
//     }
//     #endregion
//
//     #region GetListDiscountsByCatalogCodeAsync
//     [Fact]
//     public async Task GetListDiscountsByCatalogCodeAsync_ValidParams_WithNullResponse_ExpectedResult()
//     {
//         string productName = CommonHelpers.GenerateRandomString();
//
//         var call = GrpcHelpers.BuildAsyncUnaryCall<ListDetailsModel>(null!);
//
//         var discountGrpcService = new Mock<DiscountProtoService.DiscountProtoServiceClient>();
//
//         discountGrpcService.Setup(x => x.GetListDiscountsAsync(It.IsAny<GetListDiscountRequest?>(), null, null, default)).Returns(call);
//
//         var service = new DiscountGrpcService(discountGrpcService.Object);
//
//         var detail = await service.GetListDiscountsByCatalogCodeAsync(DiscountEnum.Product, new List<string> { productName });
//
//         Assert.Null(detail);
//     }
//
//     [Fact]
//     public async Task GetListDiscountsByCatalogCodeAsync_ValidParams_WithNotAnyResponse_ExpectedResult()
//     {
//         string productName = CommonHelpers.GenerateRandomString();
//         var response = new ListDetailsModel();
//
//         var call = GrpcHelpers.BuildAsyncUnaryCall(response);
//
//         var discountGrpcService = new Mock<DiscountProtoService.DiscountProtoServiceClient>();
//
//         discountGrpcService.Setup(x => x.GetListDiscountsAsync(It.IsAny<GetListDiscountRequest?>(), null, null, default)).Returns(call);
//
//         var service = new DiscountGrpcService(discountGrpcService.Object);
//
//         var detail = await service.GetListDiscountsByCatalogCodeAsync(DiscountEnum.Product, new List<string> { productName });
//
//         Assert.Null(detail);
//     }
//
//     [Fact]
//     public async Task GetListDiscountsByCatalogCodeAsync_ValidParams_ExpectedResult()
//     {
//         string productName = CommonHelpers.GenerateRandomString();
//         var response = new ListDetailsModel
//         {
//             Items =
//             {
//                 new DiscountDetailModel { CatalogName = productName, Type = Discount.Grpc.Protos.CatalogType.Product },
//                 new DiscountDetailModel { CatalogName = productName, Type = Discount.Grpc.Protos.CatalogType.Product },
//                 new DiscountDetailModel { CatalogName = productName, Type = Discount.Grpc.Protos.CatalogType.Product },
//             }
//         };
//
//         var call = GrpcHelpers.BuildAsyncUnaryCall(response);
//
//         var discountGrpcService = new Mock<DiscountProtoService.DiscountProtoServiceClient>();
//
//         discountGrpcService.Setup(x => x.GetListDiscountsAsync(It.IsAny<GetListDiscountRequest?>(), null, null, default)).Returns(call);
//
//         var service = new DiscountGrpcService(discountGrpcService.Object);
//
//         var detail = await service.GetListDiscountsByCatalogCodeAsync(DiscountEnum.Product, new List<string> { productName });
//
//         Assert.NotNull(detail);
//         Assert.Equal(3, detail.Count);
//     }
//     #endregion
//
//     #region GetAmountsAfterDiscountAsync
//     [Fact]
//     public async Task GetAmountsAfterDiscountAsync_ValidParams_ExpectedResult()
//     {
//         var categories = new List<Category>
//         {
//             new()
//                 { Id = "1", CategoryCode = "Cat1" }
//         };
//         var subCategories = new List<SubCategory>
//         {
//             new()
//                 { Id = "1", CategoryId = "1", SubCategoryCode = "Sub1" }
//         };
//         var products = new List<Product>
//         {
//             new()
//                 { Id = "1", SubCategoryId = "1", ProductCode = "Prod1" }
//         };
//
//         var response = new AmountAfterDiscountResponse
//         {
//             AmountDiscountResponse =
//             {
//                 new DiscountResponse { Type = CatalogType.Product.ToString(), CatalogCode = "Prod1.Sub1.Cat1", Amount = "10" }
//             }
//         };
//
//         var call = GrpcHelpers.BuildAsyncUnaryCall(response);
//         var discountGrpcService = new Mock<DiscountProtoService.DiscountProtoServiceClient>();
//         discountGrpcService.Setup(x => x.TotalDiscountAmountAsync(It.IsAny<ListCodeRequest>(), null, null, default)).Returns(call);
//
//         var service = new DiscountGrpcService(discountGrpcService.Object);
//
//         var result = await service.GetAmountsAfterDiscountAsync(categories, subCategories, products);
//
//         Assert.NotNull(result);
//         Assert.Equal(10, result[0].Amount);
//     }
//
//     [Fact]
//     public async Task GetAmountsAfterDiscountAsync_NullCategoryList_ThrowException()
//     {
//         var service = new DiscountGrpcService(new Mock<DiscountProtoService.DiscountProtoServiceClient>().Object);
//
//         await Assert.ThrowsAsync<ArgumentException>(async () =>
//         {
//             _ = await service.GetAmountsAfterDiscountAsync(null!, new List<SubCategory>(), new List<Product>());
//         });
//     }
//
//     [Fact]
//     public async Task GetAmountsAfterDiscountAsync_EmptyCategoryList_ThrowException()
//     {
//         var service = new DiscountGrpcService(new Mock<DiscountProtoService.DiscountProtoServiceClient>().Object);
//
//         await Assert.ThrowsAsync<ArgumentException>(async () =>
//         {
//             _ = await service.GetAmountsAfterDiscountAsync(new List<Category>(), new List<SubCategory>(), new List<Product>());
//         });
//     }
//
//     [Fact]
//     public async Task GetAmountsAfterDiscountAsync_NullSubCategoryList_ThrowException()
//     {
//         var service = new DiscountGrpcService(new Mock<DiscountProtoService.DiscountProtoServiceClient>().Object);
//
//         await Assert.ThrowsAsync<ArgumentException>(async () =>
//         {
//             _ = await service.GetAmountsAfterDiscountAsync(new List<Category> { new() }, null!, new List<Product>());
//         });
//     }
//
//     [Fact]
//     public async Task GetAmountsAfterDiscountAsync_EmptySubCategoryList_ThrowException()
//     {
//         var service = new DiscountGrpcService(new Mock<DiscountProtoService.DiscountProtoServiceClient>().Object);
//
//         await Assert.ThrowsAsync<ArgumentException>(async () =>
//         {
//             _ = await service.GetAmountsAfterDiscountAsync(new List<Category> { new() }, new List<SubCategory>(), new List<Product>());
//         });
//     }
//
//     [Theory]
//     [ClassData(typeof(TheoryDataHelpers<Product>))]
//     public async Task GetAmountsAfterDiscountAsync_NullOrEmptyProductList_ThrowException(List<Product>? products)
//     {
//         var service = new DiscountGrpcService(new Mock<DiscountProtoService.DiscountProtoServiceClient>().Object);
//
//         await Assert.ThrowsAsync<ArgumentException>(async () =>
//         {
//             _ = await service.GetAmountsAfterDiscountAsync(new List<Category> { new() }, new List<SubCategory> { new() }, products!);
//         });
//     }
//     #endregion
// }