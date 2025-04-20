using MediatR;

namespace Ordering.Application.Features.Commands.DeleteOrder;

public class DeleteOrderCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
