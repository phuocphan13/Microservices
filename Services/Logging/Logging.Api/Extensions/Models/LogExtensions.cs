using ApiClient.Logging.Models;
using Logging.Domain.Entities;
using Logging.Domain.Enums;

namespace Logging.Extensions.Models;

public static class LogExtensions
{
    public static Log ToLog(this CreateLogRequestBody requestBody)
    {
        ArgumentNullException.ThrowIfNull(requestBody);

        return new Log
        {
            Text = requestBody.Text,
            CreatedAt = requestBody.CreatedAt,
            CreatedBy = requestBody.CreatedBy,
            Type = (LogType)requestBody.Type,
            Meter = (LogMeter)requestBody.Meter,
            ObjectName = requestBody.ObjectName
        };
    } 
}