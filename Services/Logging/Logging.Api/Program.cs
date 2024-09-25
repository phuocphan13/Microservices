using Logging.Extensions;
using Logging.Middlewares;
using Worker;
using Platform;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services
    .AddPlatformCommonServices()
    .AddWorkerServices(builder.Configuration)
    .AddServiceDependencies()
    .AddOptions(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseMigrationMiddleware();
app.UseHttpsRedirection();

app.Run();