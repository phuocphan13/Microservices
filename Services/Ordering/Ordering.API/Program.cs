using EventBus.Messages;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Worker;
using Worker.Services;

var builder = WebApplication.CreateBuilder(args);
var isRebuildSchema = bool.Parse(builder.Configuration["ConnectionStrings:IsRebuildSchema"]);
var isRebuildWorkerSchema = bool.Parse(builder.Configuration["Worker:IsRebuildSchema"]);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplicationServices()
    .AddEventBusServices()
    .AddWorkerServices(builder.Configuration)
    .AddThirdParties(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration);

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

await app.InitializeWorkerDbContextsAsync(isRebuildWorkerSchema);

app.Run();