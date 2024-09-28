using ApiClient.Logging.Models;
using Logging.Domain.Repositories;
using Logging.Extensions.Models;

namespace Logging.Services;

public interface ILogService
{
    Task<bool> CreateLogsAsync(IEnumerable<CreateLogRequestBody> requestBodies, CancellationToken cancellationToken = default);
}

public class LogService : ILogService
{
    private readonly ILogRepository _logRepository;

    public LogService(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public async Task<bool> CreateLogsAsync(IEnumerable<CreateLogRequestBody> requestBodies, CancellationToken cancellationToken)
    {
        var logs = requestBodies.Select(x => x.ToLog());
        
        var result = await _logRepository.CreateLogsAsync(logs, cancellationToken);

        return result;
    }
}