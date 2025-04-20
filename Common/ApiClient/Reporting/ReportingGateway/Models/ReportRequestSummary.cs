using ApiClient.Reporting.Enums;

namespace ApiClient.Reporting.ReportingGateway.Models;

public class ReportRequestSummary
{
    public string ReportId { get; set; } = null!;
    
    public ReportStatus Status { get; set; }
    
    public DateTime LastTimeUpdated { get; set; }
}