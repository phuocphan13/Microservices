using ApiClient;
using Basket.API.Extensions.AppBuilder;
using EventBus.Messages;
using IdentityServer.Common;
using Platform;

var builder = WebApplication.CreateBuilder(args);

//Config Redis
builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration["CacheSettings:ConnectionString"];
});

// builder.Services.AddCustomAuthenticate(builder.Configuration);
// builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllers();

builder.Services
    .AddPlatformCommonServices()
    .AddServiceDependency()
    .AddThirdParty(builder.Configuration)
    .AddCatalogInternalClient()
    .AddEventBusServices();

//builder.Services.AddMassTransitHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();
