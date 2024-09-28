using System.Data;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Platform.Configurations.Options;
using Platform.Database.Entity.SQL;

namespace Logging.Domain.Repositories;

public class BaseRepository
{
    private readonly LoggingDbOptions _options;

    public BaseRepository(IOptions<LoggingDbOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options.Value;
    }
    
    private MySqlConnection GetConnection()
    {
        var connection = new MySqlConnection(_options.ConnectionString);
        
        return connection;
    }

    protected async Task<IEnumerable<T>> GetAllAsync<T>(string tableName)
        where T: BaseIdEntity, new()
    {
        var connection = GetConnection();
        string query = $"SELECT * FROM {tableName}";

        var result = await connection.QueryAsync<T>(query, CommandType.Text);

        return result;
    }
    
    protected async Task<bool> CreateAsync(string query, DynamicParameters? parameters)
    {
        var connection = GetConnection();
        var result = await connection.ExecuteAsync(query, parameters, null, null, CommandType.Text);
        
        return result > 0;
    }
}