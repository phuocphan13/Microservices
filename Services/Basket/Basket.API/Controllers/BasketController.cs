using ApiClient.Basket.Events.CheckoutEvents;
using AutoMapper;
using Basket.API.Entitites;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
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
    private readonly IBus _bus;

    public BasketController(IBasketService basketService, ILogger<BasketController> logger, IMapper mapper, IPublishEndpoint publishEndpoint, IBus bus)
        : base(logger)
    {
        _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _bus = bus;
    }

    // [Authorize]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetBasket(string userId, CancellationToken cancellationToken)
    {
        var result = await _basketService.GetBasketAsync(userId, cancellationToken);

        if (result is null)
        {
            return NotFound($"Not found Basket with Username: {userId}");
        }
        
        //For user first time to go to Basket
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> SaveBasket([FromBody] SaveBasketRequestBody basket, CancellationToken cancellationToken)
    {
        if (basket is null)
        {
            return BadRequest("Basket is not allowed Null.");
        }

        if (string.IsNullOrWhiteSpace(basket.UserId))
        {
            return BadRequest("UserId is not allowed Null or Empty.");
        }

        var result = await _basketService.SaveBasketAsync(basket, cancellationToken);

        if (result is null)
        {
            _logger.LogError($"Basket with user name: {basket.UserName}, not found.");
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteBasket(string userId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return BadRequest("UserId is not allowed Null or Empty.");
        }
        
        await _basketService.DeleteBasketAsync(userId, cancellationToken);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout, CancellationToken cancellationToken)
    {
        var basket = await _basketService.GetBasketDetailAsync(basketCheckout.UserId, cancellationToken);
        
        if (basket == null)
        {
            return BadRequest();
        }

        // send checkout event to rabbitmq
        var eventMessage = _mapper.Map<BasketCheckoutMessage>(basket);

        await _publishEndpoint.Publish(eventMessage, cancellationToken);

        // await _basketService.DeleteBasketAsync(basket.UserId, cancellationToken);

        return Accepted();
    }
}
