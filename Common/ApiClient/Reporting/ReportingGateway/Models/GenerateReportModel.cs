using ApiClient.Reporting.Enums;

namespace ApiClient.Reporting.ReportingGateway.Models;

public class GenerateReportModel
{
    public ReportStatus Status { get; set; }
    
    public string ErrorMessage { get; set; } = null!;
}