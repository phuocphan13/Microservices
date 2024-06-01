using ApiClient;
using Discount.Domain;
using Discount.Domain.Common.InitializeDB;
using Discount.Grpc.GrpcServices;
using Discount.Grpc.Services;
using Platform;

var builder = WebApplication.CreateBuilder(args);
var isRebuildSchema = builder.Configuration.GetValue<bool>(Platform.Constants.DatabaseConst.ConnectionSetting.Postgres.IsRebuildSchema);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddHttpClient();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDiscountCommonServices();
builder.Services.AddPlatformCommonServices();
builder.Services.AddCatalogServices();

var app = builder.Build();

await app.InitializeDiscountDbContextsAsync(builder.Configuration, isRebuildSchema);

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.MapGrpcService<CouponService>();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
