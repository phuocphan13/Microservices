using Dapper;
using Logging.Domain.Consts;
using Logging.Domain.Entities;
using Logging.Domain.Enums;
using Microsoft.Extensions.Options;
using Platform.Configurations.Options;

namespace Logging.Domain.Repositories;

public interface ILogRepository
{
    Task<IEnumerable<Log>> GetAllLogsAsync(CancellationToken cancellationToken = default);
    Task<bool> CreateLogsAsync(IEnumerable<Log> logs, CancellationToken cancellationToken = default);
}

public class LogRepository : BaseRepository, ILogRepository
{
    public LogRepository(IOptions<LoggingDbOptions> options)
        : base(options)
    {
        ArgumentNullException.ThrowIfNull(options);
    }
    
    public async Task<IEnumerable<Log>> GetAllLogsAsync(CancellationToken cancellationToken)
    {
        var result = await GetAllAsync<Log>(DatabaseConstants.Table.Log);
        
        return result;
    }
    
    public async Task<bool> CreateLogsAsync(IEnumerable<Log> logs, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(logs);

        var query = $"INSERT INTO {DatabaseConstants.Table.Log} (Text, Type, LogMeter, ObjectName, CreatedAt, CreatedBy) VALUES";

        var parameters = new List<string>();
        
        foreach (var log in logs)
        {
            parameters.Add($"('{log.Text}', {(int)log.Type}, {(int)log.Meter}, '{log.ObjectName}', '{log.CreatedAt}', 'Admin')");
        }

        query += string.Join(",", parameters);

        var result = await CreateAsync(query, null);

        return result;
    }
}