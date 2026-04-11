using Course_Selling_System.Dtos;
using Course_Selling_System.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Course_Selling_System.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CourseListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPublished(CancellationToken ct = default)
        {
            var courses = await _courseService.GetPublishedAsync(ct);
            return Ok(courses);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CourseDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
        {
            var courses = await _courseService.GetPushlishedByIdAsync(id, ct);
            if(courses is null)
            {
                return NotFound();
            }
            return Ok(courses);
        }
    }
}
