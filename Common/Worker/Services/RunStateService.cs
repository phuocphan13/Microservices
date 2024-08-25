using Microsoft.EntityFrameworkCore;
using Worker.Entities;

namespace Worker.Services;

public interface IRunStateService
{
    Task<bool> IsRunningAsync(string jobName, CancellationToken cancellationToken = default);
    Task SaveJobRunningInfoAsync(string jobName, bool isSuccess, string errorMessage, CancellationToken cancellationToken = default);
}

public class RunStateService : IRunStateService
{
    public async Task<bool> IsRunningAsync(string jobName, CancellationToken cancellationToken)
    {
        var context = new WorkerContext();
        
        var job = await context.Jobs.AsNoTracking().FirstOrDefaultAsync(x => x.Name == jobName, cancellationToken);
        
        if (job is null)
        {
            return false;
        }
        
        var lastRun = await context.JobRunHistories
            .Where(x => x.JobExternalId == job.ExternalId)
            .OrderByDescending(x => x.TimeStamp)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (lastRun is null)
        {
            return false;
        }
        
        return lastRun.TimeStamp > DateTime.UtcNow.AddMinutes(-30);
    }
    
    public async Task SaveJobRunningInfoAsync(string jobName, bool isSuccess, string errorMessage, CancellationToken cancellationToken)
    {
        var context = new WorkerContext();
        
        var job = await context.Jobs.FirstOrDefaultAsync(x => x.Name == jobName, cancellationToken) 
                  ?? await CreateJobAsync(jobName, cancellationToken);

        var runHistory = new JobRunHistory
        {
            TimeStamp = DateTime.UtcNow,
            State = isSuccess ? RunStateEnum.Succeed : RunStateEnum.Failed,
            JobExternalId = job.ExternalId,
            ErrorMessage = errorMessage
        };
        
        await context.JobRunHistories.AddAsync(runHistory, cancellationToken);
        
        await context.SaveChangesAsync(cancellationToken);
    }
    
    private async Task<Job> CreateJobAsync(string jobName, CancellationToken cancellationToken)
    {
        var context = new WorkerContext();
        
        var job = new Job
        {
            ExternalId = Guid.NewGuid(),
            CreatedDate = DateTime.Now,
            CreatedBy = "Admin",
            Name = jobName,
            LastModifiedDate = DateTime.Now,
            LastModifiedBy = "Admin",
        };
        
        await context.Jobs.AddAsync(job, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return job;
    }
}