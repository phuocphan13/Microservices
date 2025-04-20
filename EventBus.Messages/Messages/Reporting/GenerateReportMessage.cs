namespace EventBus.Messages.Messages.Reporting;

public class GenerateReportMessage : BaseMessage
{
    public string ReportId { get; set; } = null!;
    
    public string UserId { get; set; } = null!;
}