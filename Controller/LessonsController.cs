using Course_Selling_System.Dtos;
using Course_Selling_System.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Course_Selling_System.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }
        [HttpGet("course/{courseId:int}")]
        [ProducesResponseType(typeof(IEnumerable<LessonDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLessonsForPublishedCourse(int courseId, CancellationToken ct = default)
        {
            var lessons = await _lessonService.GetLessonsForPublishedCourseAsyunc(courseId, ct);
            if (lessons == null) return NotFound();
            return Ok(lessons);
        }
    }
}
