using Basket.API.Entitites;
using Microsoft.AspNetCore.Mvc;
using ApiClient.Basket.Models;
using Basket.API.Services;
using Platform.ApiBuilder;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class BasketController : ApiController
{
    private readonly IBasketService _basketService;
    private readonly ILogger<BasketController> _logger;

    public BasketController(IBasketService basketService, ILogger<BasketController> logger)
        : base(logger)
    {
        _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetBasket(string userId, CancellationToken cancellationToken)
    {
        var result = await _basketService.GetBasketAsync(userId, cancellationToken);

        if (result is null)
        {
            return NotFound($"Not found Basket with Username: {userId}");
        }
        
        return Ok(result);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetBasketPreCheckout(string userId, CancellationToken cancellationToken)
    {
        var result = await _basketService.GetBasketPreCheckoutAsync(userId, cancellationToken);

        if (result is null)
        {
            return NotFound($"Not found Basket with Username: {userId}");
        }

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
        if (basketCheckout is null)
        {
            return BadRequest("BasketCheckout is not allowed Null.");
        }

        if (string.IsNullOrWhiteSpace(basketCheckout.UserId))
        {
            return BadRequest("UserId is not allowed Null or Empty.");
        }
        
        var isSucceed = await _basketService.CheckoutBasketAsync(basketCheckout.UserId, cancellationToken);

        if (!isSucceed)
        {
            return Problem("Checkout failed.");
        }

        return Accepted();
    }
}
