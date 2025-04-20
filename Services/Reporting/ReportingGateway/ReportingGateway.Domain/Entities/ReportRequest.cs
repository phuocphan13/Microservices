using ApiClient.Reporting.Enums;
using Platform.Database.Entity.MongoDb;
using Platform.Database.MongoDb;

namespace ReportingGateway.Domain.Entities;

[BsonCollection("ReportRequest")]
public class ReportRequest : BaseEntity
{
    public string UserId { get; set; } = null!;
    
    public ReportType ReportType { get; set; }
    
    public ReportStatus Status { get; set; }

    public DateTime RequestedDate { get; set; }
    
    public DateTime? InProgressDate { get; set; } = null;

    public DateTime? FinishedDate { get; set; } = null;
}