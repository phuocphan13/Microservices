using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using Ordering.Infrastructure.Database;
using Ordering.Infrastructure.Mail;
using Ordering.Infrastructure.Persistence;
using Platform.Database.Helpers;

namespace Ordering.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderContext>(options =>
            options.UseSqlServer(configuration["ConnectionStrings:OrderingConnectionString"]));

        services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IEmailService, EmailService>();

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
