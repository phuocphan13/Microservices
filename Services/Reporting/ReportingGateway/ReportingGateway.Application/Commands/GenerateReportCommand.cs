using ApiClient.Reporting.Enums;
using ApiClient.Reporting.ReportingGateway.Models;
using EventBus.Messages.Messages.Reporting;
using EventBus.Messages.Services;
using MediatR;
using Platform.Database.MongoDb;
using ReportingGateway.Application.Extensions;
using ReportingGateway.Domain.Entities;

namespace ReportingGateway.Application.Commands;

public class GenerateReportCommand : IRequest<GenerateReportModel>
{
    public  int ReportType { get; set; }
    
    public string UserId { get; set; } = null!;
}

public class GenerateReportCommandHandler : IRequestHandler<GenerateReportCommand, GenerateReportModel>
{
    private readonly IRepository<ReportRequest> _reportRequestRepository;
    private readonly IQueueService _queueService;

    public GenerateReportCommandHandler(IRepository<ReportRequest> reportRequestRepository, IQueueService queueService)
    {
        _reportRequestRepository = reportRequestRepository;
        _queueService = queueService;
    }

    public async Task<GenerateReportModel> Handle(GenerateReportCommand request, CancellationToken cancellationToken)
    {
        var report = new GenerateReportModel();
        
        var entity = request.ToReportRequest();
        
        entity = await _reportRequestRepository.CreateEntityAsync(entity, cancellationToken);

        if (entity.Id is null)
        {
            report.ErrorMessage = "Create Report Request failed";

            return report;
        }
        
        report.Status = ReportStatus.Requested;
        
        var queueMessage = new GenerateReportMessage
        {
            ReportId = entity.Id,
            UserId = entity.UserId,
        };
        
        await _queueService.SendMessageAsync(queueMessage);

        return report;
    }
}