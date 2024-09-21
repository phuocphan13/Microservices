using Microsoft.Extensions.Options;
using Platform.Configurations.Options;

namespace Logging.Domain.Repositories;

public interface ILogRepository
{
    
}

public class LogRepository : BaseRepository, ILogRepository
{
    public LogRepository(IOptions<LoggingDbOptions> options)
        : base(options)
    {
        ArgumentNullException.ThrowIfNull(options);
    }
}