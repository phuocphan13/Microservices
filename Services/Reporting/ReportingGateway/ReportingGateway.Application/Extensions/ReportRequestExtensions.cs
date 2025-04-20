using ApiClient.Reporting.Enums;
using ReportingGateway.Application.Commands;
using ReportingGateway.Domain.Entities;

namespace ReportingGateway.Application.Extensions;

public static class ReportRequestExtensions
{
    public static ReportRequest ToReportRequest(this GenerateReportCommand request)
    {
        return new ReportRequest
        {
            UserId = request.UserId,
            ReportType = (ReportType)request.ReportType,
            RequestedDate = DateTime.Now,
            Status = ReportStatus.Requested,
        };
    }
}