using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using MediatR;
using Ordering.Application.Behaviours;
using Ordering.Application.Services;
using Ordering.Application.WorkerServices;

namespace Ordering.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        //Services
        services.AddScoped<IOrderStatusService, OrderStatusService>();
        
        // Jobs
        services.AddSingleton<IBasketStateJobService, BasketStateJobService>();

        return services;
    }
}
