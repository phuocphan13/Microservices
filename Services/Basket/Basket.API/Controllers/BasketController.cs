using AutoMapper;
using Basket.API.Entitites;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ApiClient.Basket.Models;
using Basket.API.Services;
using Microsoft.AspNetCore.Authorization;
using Platform.ApiBuilder;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class BasketController : ApiController
{
    private readonly IBasketService _basketService;
    private readonly ILogger<BasketController> _logger;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public BasketController(IBasketService basketService, ILogger<BasketController> logger, IMapper mapper, IPublishEndpoint publishEndpoint)
        : base(logger)
    {
        _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    }

    // [Authorize]
    [HttpGet("GetBasket/{userName}")]
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
    public async Task<IActionResult> SaveBasket([FromBody] SaveCartRequestBody cart, CancellationToken cancellationToken)
    {
        if (cart is null)
        {
            return BadRequest("Cart is not allowed Null.");
        }

        if (string.IsNullOrWhiteSpace(cart.UserId))
        {
            return BadRequest("UserNmaId is not allowed Null or Empty.");
        }

        var result = await _basketService.SaveCartAsync(cart, cancellationToken);

        if (result is null)
        {
            _logger.LogError($"Basket with user name: {cart.UserName}, not found.");
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete("DeleteBasket/{userName}")]
    public async Task<IActionResult> DeleteBasket(string userName, CancellationToken cancellationToken)
    {
        await _basketService.DeleteBasketAsync(userName, cancellationToken);
        return Ok();
    }

    [HttpPost]
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
