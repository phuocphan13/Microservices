using ApiClient;
using Discount.Domain;
using Discount.Domain.Common.InitializeDB;

var builder = WebApplication.CreateBuilder(args);
var isRebuildSchema = builder.Configuration.GetValue<bool>(Platform.Constants.DatabaseConst.ConnectionSetting.Postgres.IsRebuildSchema);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCatalogApiClient();

builder.Services.AddDiscountCommonServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

await app.InitializeDiscountDbContextsAsync(builder.Configuration, isRebuildSchema);

app.Run();
