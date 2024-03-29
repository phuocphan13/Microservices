// namespace Catalog.API.Common.Extensions;
//
// public static class HealthChecksExtensions
// {
//     public static IServiceCollection AddDefaultHealthChecks(this IServiceCollection services)
//     {
//         services.AddHealthChecks()
//             .AddCheck<ConfigurationHealthCheck>("conf", tags: ["live"])
//             .AddDbContextCheck<AppDbContext>("db", tags: ["ready"]);
//
//         return services;
//     }
//
//     public static IEndpointRouteBuilder MapDefaultHealthChecksGroup(this IEndpointRouteBuilder endpoints, string pathGroup)
//     {
//         var group = endpoints.MapGroup(pathGroup);
//
//         group.MapHealthChecks("/ready");
//
//         group.MapHealthChecks("/live", new HealthCheckOptions
//         {
//             Predicate = r => r.Tags.Contains("live"),
//         });
//
//         return endpoints;
//     }
// }
