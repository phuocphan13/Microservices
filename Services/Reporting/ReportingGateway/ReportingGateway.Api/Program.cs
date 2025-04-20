using EventBus.Messages;
using Platform;
using ReportingGateway.Api.Extensions;
using ReportingGateway.Application;
using ReportingGateway.Domain;
using ReportingGateway.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services
    .AddPlatformCommonServices()
    .AddDependencyApplication()
    .AddDependencyDomain()
    .AddEventBusServices()
    .AddDependencyInfrastructure(builder.Configuration)
    .AddThirdParties(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();