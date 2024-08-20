using Microsoft.EntityFrameworkCore;
using Worker.Entities;

namespace Worker.Services;

public interface IRunStateService
{
    Task SaveJobRunningInfoAsync(string jobName, bool isSuccess, string errorMessage, CancellationToken cancellationToken = default);
}

public class RunStateService : IRunStateService
{
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