using AutoMapper;
using Basket.API.Entitites;
using Basket.API.GrpcServices;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ApiClient.Basket.Models;
using Basket.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;
    private readonly ILogger<BasketController> _logger;
    private readonly IDiscountGrpcService _discountGrpcService;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public BasketController(IBasketService basketService, ILogger<BasketController> logger, IDiscountGrpcService discountGrpcService, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    }

    [Authorize]
    [HttpGet("{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(CartDetail), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBasket(string userName, CancellationToken cancellationToken)
    {
        var result = await _basketService.GetBasketAsync(userName, cancellationToken);

        if (result is null)
        {
            return NotFound($"Not found Basket with Username: {userName}");
        }
        
        //For user first time to go to Basket
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateBasket([FromBody] UpdateCartRequestBody basket, CancellationToken cancellationToken)
    {
        foreach (var item in basket.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductCode!);
            // item.Price -= coupon.Amount;
        }

        var result = await _basketService.UpdateBasketAsync(basket, cancellationToken);

        if (result is null)
        {
            _logger.LogError($"Basket with user name: {basket.UserName}, not found.");
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string userName, CancellationToken cancellationToken)
    {
        await _basketService.DeleteBasketAsync(userName, cancellationToken);
        return Ok();
    }

    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout, CancellationToken cancellationToken)
    {
        // get existing basket with total price
        var basket = await _basketService.GetBasketAsync(basketCheckout.UserName, cancellationToken);

        if (basket == null || string.IsNullOrWhiteSpace(basket.UserName))
        {
            return BadRequest();
        }

        // send checkout event to rabbitmq
        var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
        eventMessage.TotalPrice = basket.TotalPrice;

        await _publishEndpoint.Publish(eventMessage, cancellationToken);

        // remove the basket
        await _basketService.DeleteBasketAsync(basket.UserName, cancellationToken);

        return Accepted();
    }
}
