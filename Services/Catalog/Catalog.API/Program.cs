using Catalog.API.Common;
using Catalog.API.Common.Consts;
using Catalog.API.Common.Extensions;
using Catalog.API.Repositories;
using Catalog.API.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var isRebuildSchema = builder.Configuration.GetValue<bool>(DatabaseConst.CollectionName.IsRebuildSchema);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddSingleton<IDatabaseSettings>(serviceProvider =>
    serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

//Services
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

await app.InitializePlatformDbContextsAsync(builder.Configuration, isRebuildSchema);

await app.RunAsync();
