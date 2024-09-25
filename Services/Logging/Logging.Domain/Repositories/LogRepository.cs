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
    
    public async Task CreateLogsAsync(IEnumerable<Log> logs, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(logs);
        
        foreach (var log in logs)
        {
            var query = $"INSERT INTO {DatabaseConstants.Table.Log} (Text, Type, LogMeter, ObjectName, CreatedAt, CreatedBy) VALUES (@Text, @Type, @LogMeter, @ObjectName, @CreatedAt, @CreatedBy)";
            var parameters = new DynamicParameters();
            // parameters.Add("@Text", log.Message);
            // parameters.Add("@Type", LogType.Info);
            // parameters.Add("@LogMeter", log.CreatedAt);
            // parameters.Add("@ObjectName", log.Message);
            // parameters.Add("@CreatedAt", DateTime.UtcNow);
            // parameters.Add("@CreatedBy", "Addin");
            
            await CreateAsync(query, parameters);
        }
    }
        
}