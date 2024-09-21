using Microsoft.Extensions.Options;
using Platform.Configurations.Options;

namespace Logging.Domain.Repositories;

public class BaseRepository
{
    private readonly LoggingDbOptions _options;

    public BaseRepository(IOptions<LoggingDbOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options.Value;
    }

    public async Task GetAllLogsAsync()
    {
        
    }
}