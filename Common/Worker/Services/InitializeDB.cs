using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Worker.Entities;

namespace Worker.Services;

public static class InitializeDB
{
    public static async Task InitializeWorkerDbContextsAsync(
        this IApplicationBuilder _,
        bool isRebuildSchema = true,
        CancellationToken cancellationToken = default)
    {
        if (isRebuildSchema)
        {
            var dbContext = new WorkerContext();
            await dbContext.Database.MigrateAsync(cancellationToken);

            var jobs = GetJobs();

            await dbContext.Jobs.AddRangeAsync(jobs, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private static List<Job> GetJobs()
    {
        return new List<Job>
        {
            new()
            {
                ExternalId = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                CreatedBy = "Admin",
                Name = "AcceptOrder",
                LastModifiedDate = DateTime.Now,
                LastModifiedBy = "Admin",
            },
            new()
            {
                ExternalId = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                CreatedBy = "Admin",
                Name = "UpdateCache",
                LastModifiedDate = DateTime.Now,
                LastModifiedBy = "Admin",
            },
        };
    }
}