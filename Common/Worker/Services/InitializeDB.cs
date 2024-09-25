using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Platform.Configurations.Options;
using Worker.Entities;

namespace Worker.Services;

public static class InitializeDB
{
    public static async Task InitializeWorkerDbContextsAsync(
        this IApplicationBuilder app,
        bool isRebuildSchema = true,
        CancellationToken cancellationToken = default)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var workerOptions = scope.ServiceProvider.GetService<IOptions<WorkerOptions>>();

        if (workerOptions is null)
        {
            return;
        }
        
        if (isRebuildSchema)
        {
            var dbContext = new WorkerContext(workerOptions);
            await dbContext.Database.MigrateAsync(cancellationToken);

            var jobs = GetJobs();

            await dbContext.Jobs.AddRangeAsync(jobs, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private static List<Job> GetJobs()
    {
        return
        [
            new()
            {
                ExternalId = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                CreatedBy = "Admin",
                Name = "AcceptOrder",
                LastModifiedDate = DateTime.Now,
                LastModifiedBy = "Admin"
            },
            new()
            {
                ExternalId = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                CreatedBy = "Admin",
                Name = "UpdateCache",
                LastModifiedDate = DateTime.Now,
                LastModifiedBy = "Admin"
            }
        ];
    }
}