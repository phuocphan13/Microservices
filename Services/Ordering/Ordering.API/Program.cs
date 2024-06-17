using EventBus.Messages;
using EventBus.Messages.Extensions;
using EventBus.Messages.StateMachine;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
var isRebuildSchema = bool.Parse(builder.Configuration["Database:IsRebuildSchema"]);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplicationServices()
    .AddEventBusServices()
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<BasketCheckoutConsumer>();

builder.Services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
builder.Services.AddMassTransit(config =>
{
    config.AddConsumersFromNamespaceContaining<BasketCheckoutConsumer>();
    config.AddSagaStateMachine<BasketStateMachine, BasketStateInstance>()
        .RedisRepository(r =>
        {
            r.DatabaseConfiguration(builder.Configuration["ConnectionStrings:SagaConnectionString"]);
        });
    
    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddHostedService<MassTransitConsoleHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

if (isRebuildSchema)
{
    app.MigrateDatabase<OrderContext>((context, services) =>
    {
        var logger = services.GetService<ILogger<OrderContextSeed>>();
        OrderContextSeed.SeedAsync(context!, logger!).Wait();
    });
}

app.Run();