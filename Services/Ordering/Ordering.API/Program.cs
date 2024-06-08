using EventBus.Messages;
using EventBus.Messages.Common;
using EventBus.Messages.TestModel;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastruture;
using Ordering.Infrastruture.Persistence;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddMassTransit(config =>
{
    // config.AddConsumer<BasketCheckoutConsumer>();
    //
    // config.UsingRabbitMq((ctx, cfg) =>
    // {
    //     cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    //     cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
    //     {
    //         c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
    //     });
    // });


    config.AddConsumers(typeof(Program).Assembly);

    config.UsingRabbitMq((ctx, cfg) =>
    {
        // cfg.ConfigureEndpoints(ctx);
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.ReceiveEndpoint("Test-Message-Queue", c =>
        {
            // c.Bind("direct-queue", x =>
            // {
            //     x.ExchangeType = "direct";
            // });
            c.ConfigureConsumeTopology = false;
            c.Bind("direct-queue");
            // c.Bind<TestModel>(x =>
            // {
            //     x.ExchangeType = "direct";
            // });
            c.Consumer<TestMessageQueueConsumer>();
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MigrateDatabase<OrderContext>((context, services) =>
{
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context!, logger!).Wait();
});
app.Run();