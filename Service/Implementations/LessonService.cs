using Course_Selling_System.Dtos;
using Course_Selling_System.Models;
using Course_Selling_System.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Course_Selling_System.Service.Implementations
{
    public class LessonService : ILessonService
    {
        private readonly CourseSellingDbContext _context;
        public LessonService(CourseSellingDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<LessonDto?>> GetLessonsForPublishedCourseAsyunc(int courseId, CancellationToken ct = default)
        {
            var courseExistsPublished = await _context.Courses.AsNoTracking()
                .AnyAsync(c => c.Id == courseId && c.IsPublished, ct);

            if (!courseExistsPublished) return null;
            var lesson = await _context.Lessons.AsNoTracking()
                .AsNoTracking()
                .Where(l => l.CourseId == courseId)
                .OrderBy(l => l.OrderIndex)
                .Select(l => new LessonDto
                {
                    CourseId = l.CourseId,
                    DurationSeconds = l.DurationSeconds,
                    Id = l.Id,
                    OrderIndex = l.OrderIndex,
                    Title = l.Title,
                    VideoUrl = l.VideoUrl
                })
                .ToListAsync(ct);
            return lesson;
        }
    }
}
