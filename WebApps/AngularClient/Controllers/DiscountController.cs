using AngularClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace AngularClient.Controllers.Discount;

[ApiController]
[Route("api/[controller]/[action]")]

public class DiscountController : ControllerBase
{
    private readonly IDiscountService _discountService;

    public DiscountController (IDiscountService discountService)
    {
        _discountService = discountService;
    }

    [HttpPost("id")]
    public async Task<IActionResult> InactiveDiscount( int id, CancellationToken cancellationToken )
    {
        var result = await _discountService.InactiveDiscountAsync( id, cancellationToken );
        return Ok( result );
    }
}

