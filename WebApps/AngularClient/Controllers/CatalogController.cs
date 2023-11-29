using AngularClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace AngularClient.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CatalogController : ControllerBase
{
   private readonly ICatalogService _catalogService;

   public CatalogController(ICatalogService catalogService)
   {
      _catalogService = catalogService;
   }

   [HttpGet]
   public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
   {
      var result = await _catalogService.GetProductsAsync(cancellationToken);
   
      if (result is null)
      {
         return NotFound();
      }
   
      return Ok(result);
   }
}