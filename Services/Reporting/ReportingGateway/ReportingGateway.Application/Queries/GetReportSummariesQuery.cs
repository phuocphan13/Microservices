using ApiClient.Reporting.ReportingGateway.Models;
using MediatR;
using Platform.Database.MongoDb;
using ReportingGateway.Domain.Entities;

namespace ReportingGateway.Application.Queries;

public class GetReportSummariesQuery : IRequest<List<ReportRequestSummary>>
{
    public string UserId { get; set; } = null!;
}

public class GetReportSummariesQueryHandler : IRequestHandler<GetReportSummariesQuery, List<ReportRequestSummary>>
{
    private readonly IRepository<ReportRequest> _reportRequestRepository;

    public GetReportSummariesQueryHandler(IRepository<ReportRequest> reportRequestRepository)
    {
        _reportRequestRepository = reportRequestRepository;
    }

    public async Task<List<ReportRequestSummary>> Handle(GetReportSummariesQuery request, CancellationToken cancellationToken)
    {
        var reportRequests = await _reportRequestRepository
            .GetEntitiesQueryAsync(x => x.UserId == request.UserId, cancellationToken);

        if (reportRequests == null || !reportRequests.Any())
        {
            return [ ];
        }
        
        var reportRequestSummaries = reportRequests
            .Select(x => new ReportRequestSummary
            {
                ReportId = x.Id,
                Status = x.Status,
                LastTimeUpdated = x.FinishedDate ?? x.InProgressDate ?? x.RequestedDate,
            })
            .ToList();

        return reportRequestSummaries;
    }
}