using ApiClient.Catalog.Models.Category;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetCategoriesAsync(cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestBody requestBody, CancellationToken cancellationToken)
        {
            if (requestBody is null)
            {
                return BadRequest("RequestBody is not allowed null.");
            }

            var result = await _categoryService.CreateCategoryAsync(requestBody, cancellationToken);

            if (!result.IsSuccessCode)
            {
                if (result.InternalErrorCode == 404)
                {
                    return NotFound(result.Message);
                }

                if (result.InternalErrorCode == 500)
                {
                    return Problem(result.Message);
                }
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryByName([FromQuery] string categoryName, CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetCategoryByNameAsync(categoryName,cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryById([FromQuery] string categoryId, CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetCategoryByIdAsync(categoryId, cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken)
        {
            if (requestBody is null)
            {
                return BadRequest("RequestBody is not allowed null.");
            }

            if (string.IsNullOrWhiteSpace(requestBody.Name))
            {
                return BadRequest("Category Name is not allowed null.");
            }

            var result = await _categoryService.UpdateCategoryAsync(requestBody, cancellationToken);

            if (result is null)
            {
                return Problem($"Cannot update category with name: {requestBody.Name}");
            }

            return Ok(result);
        }
    }
}
