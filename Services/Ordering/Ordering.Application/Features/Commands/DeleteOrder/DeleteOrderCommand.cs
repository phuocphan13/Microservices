using MediatR;

namespace Ordering.Application.Features.Commands.DeleteOrder;

public class DeleteOrderCommand : IRequest
{
    public int Id { get; set; }
}
