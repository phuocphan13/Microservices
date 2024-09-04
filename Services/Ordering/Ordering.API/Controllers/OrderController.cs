using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Commands.CheckoutOrder;
using Ordering.Application.Features.Commands.DeleteOrder;
using Ordering.Application.Features.Commands.UpdateOrder;
using Ordering.Application.Features.Queries.GetOrderList;
using System.Net;
using ApiClient.Catalog.Product.Events;
using EventBus.Messages.Services;

namespace Ordering.API.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IQueueService _queueService;

    public OrderController(IMediator mediator, IQueueService queueService)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _queueService = queueService;
    }

    [HttpGet("{userName}", Name = "GetOrder")]
    [ProducesResponseType(typeof(IEnumerable<OrdersVm>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrdersVm>>> GetOrdersByUserName(string userName)
    {
        var query = new GetOrdersListQuery(userName);
        var orders = await _mediator.Send(query);
        return Ok(orders);
    }

    // testing purpose
    [HttpPost(Name = "CheckoutOrder")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut(Name = "UpdateOrder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteOrder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        var command = new DeleteOrderCommand() { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> Test([FromQuery] string key, CancellationToken cancellationToken)
    {
        await _queueService.SendFanoutMessageAsync(new ProductBalanceUpdateMessage()
        {
            ReceiptNumber = key,
            UserId = "test",
            UserName = "test",
            MemberId = "test",
            EventId = "test"
        }, cancellationToken);

        return Ok();
    }
}
