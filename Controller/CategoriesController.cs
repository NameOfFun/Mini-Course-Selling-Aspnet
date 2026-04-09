using Course_Selling_System.Dtos;
using Course_Selling_System.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Course_Selling_System.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCategories(CancellationToken ct = default)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(ct);
            return Ok(categories);
        }
    }
}
