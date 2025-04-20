using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using MediatR;
using Ordering.Application.Behaviours;
using Ordering.Application.Features.Commands.CheckoutOrder;
using Ordering.Application.Features.Commands.DeleteOrder;
using Ordering.Application.Features.Commands.FailureOrder;
using Ordering.Application.Features.Commands.UpdateOrder;
using Ordering.Application.Features.Queries.GetOrderList;
using Ordering.Application.Services;
using Ordering.Application.WorkerServices;

namespace Ordering.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg =>
        {
            // Commands
            cfg.RegisterServicesFromAssemblyContaining<CheckoutOrderCommandHandler>();
            cfg.RegisterServicesFromAssemblyContaining<DeleteOrderCommandHandler>();
            cfg.RegisterServicesFromAssemblyContaining<FailureOrderCommandHandler>();
            cfg.RegisterServicesFromAssemblyContaining<UpdateOrderCommandHandler>();

            // Queries
            cfg.RegisterServicesFromAssemblyContaining<GetOrdersListQueryHandler>();
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        //Services
        services.AddScoped<IOrderStatusService, OrderStatusService>();
        
        // Jobs
        services.AddSingleton<IBasketStateJobService, BasketStateJobService>();

        return services;
    }
}
