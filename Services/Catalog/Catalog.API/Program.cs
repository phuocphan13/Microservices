using Catalog.API.Common.Extensions;
using Catalog.API.Repositories;
using Catalog.API.Services;
using Catalog.API.Services.Grpc;
using Discount.Grpc.Protos;
using IdentityServer.Common;
using Platform;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddHealthChecks();

var isRebuildSchema = builder.Configuration.GetValue<bool>(Platform.Constants.DatabaseConst.ConnectionSetting.MongoDB.IsRebuildSchema);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddPlatformCommonServices();

builder.Services.AddCustomAuthenticate(builder.Configuration);
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

//Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
builder.Services.AddScoped<IValidationService, ValidationService>();

builder.Services.AddScoped<IDiscountGrpcService, DiscountGrpcService>();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(x => x.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));

var app = builder.Build();

//Todo adding healthcheck later
// app.MapHealthChecks("/healthz");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.InitializePlatformDbContextsAsync(builder.Configuration, isRebuildSchema);

await app.RunAsync();

public partial class Program
{
    protected Program()
    {
    }
}
