using MediatR;

namespace Ordering.Application.Features.Commands.FailureOrder;

public class FailureOrderCommand : IRequest<bool>
{
    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string ReceiptNumber { get; set; } = null!;
}