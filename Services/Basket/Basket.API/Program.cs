using ApiClient;
using Basket.API.Extensions.AppBuilder;
using EventBus.Messages;
using Platform;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services
    .AddPlatformCommonServices()
    .AddServiceDependency()
    .AddThirdParty(builder.Configuration)
    .AddCatalogInternalClient()
    .AddEventBusServices()
    .AddOptions(builder.Configuration);

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
