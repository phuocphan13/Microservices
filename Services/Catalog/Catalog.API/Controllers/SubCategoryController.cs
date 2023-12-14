using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService ?? throw new ArgumentNullException(nameof(subCategoryService));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetSubCategories(CancellationToken cancellationToken)
        {
            var result = await _subCategoryService.GetSubCategoriesAsync(cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetSubCategoryByName(string name, CancellationToken cancellationToken)
        {
            var result = await _subCategoryService.GetSubCategoryByNameAsync(name, cancellationToken);
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetSubCategoryById(string id, CancellationToken cancellationToken)
        {
            var result = await _subCategoryService.GetSubCategoryByIdAsync(id, cancellationToken);
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
