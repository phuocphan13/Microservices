using ApiClient.Reporting.Enums;
using ApiClient.Reporting.ReportingGateway.RequestBodies;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportingGateway.Application.Commands;
using ReportingGateway.Application.Queries;

namespace ReportingGateway.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetReportSummaries([FromQuery] string userId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return BadRequest("UserId cannot be null or empty.");
        }
        
        var query = new GetReportSummariesQuery
        {
            UserId = userId
        };
        
        var reportSummaries = await _mediator.Send(query, cancellationToken);
        
        return Ok(reportSummaries);
    }

    [HttpPost]
    public async Task<IActionResult> GenerateReport([FromQuery] int reportType, [FromBody] GenerateReportRequestBody requestBody, CancellationToken cancellationToken)
    {
        if (requestBody is null)
        {
            return BadRequest("Request body cannot be null.");
        }
        
        if (string.IsNullOrEmpty(requestBody.UserId))
        {
            return BadRequest("UserId cannot be null or empty.");
        }
        
        if (reportType == 0)
        {
            return BadRequest("Report type cannot be empty.");
        }
        
        var command = new GenerateReportCommand
        {
            UserId = requestBody.UserId,
            ReportType = reportType
        };
        
        var report = await _mediator.Send(command, cancellationToken);

        if (report is null || !string.IsNullOrWhiteSpace(report.ErrorMessage))
        {
            return Problem("An error occurred while generating the report.");
        }
        
        return Ok(report);
    }
}