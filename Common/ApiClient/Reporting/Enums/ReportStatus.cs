namespace ApiClient.Reporting.Enums;

public enum ReportStatus
{
    Requested = 1,
    InProgress = 2,
    Completed = 3,
    PartiallyCompleted = 4,
    Failed = 5,
    Cancelled = 6,
}