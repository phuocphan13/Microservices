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
        var job = await context.Jobs.FirstOrDefaultAsync(x => x.Name == jobName, cancellationToken);
        
        if (job == null)
        {
            throw new Exception($"Job with name {jobName} not found");
        }
        
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
}