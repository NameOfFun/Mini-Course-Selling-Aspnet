using Course_Selling_System.Dtos;
using Course_Selling_System.Models;
using Course_Selling_System.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Course_Selling_System.Service.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly CourseSellingDbContext _context;
        public CourseService(CourseSellingDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CourseListDto>> GetPublishedAsync(CancellationToken ct = default)
        {
            return await _context.Courses
                .AsNoTracking()
                .Where(c => c.IsPublished)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CourseListDto 
                { 
                    Id = c.Id,
                    Title = c.Title,
                    Price = c.Price,
                    ThumbnaiUrl = c.ThumbnailUrl,
                    CategoryName = c.Category.Name,
                    InstructorName = c.Instructor.FullName,
                    CreateAt = c.CreatedAt
                })
                .ToListAsync(ct);
        }

        public async Task<CourseDetailDto?> GetPushlishedByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Courses
                .AsNoTracking()
                .Where(c => c.IsPublished && c.Id == id)
                .Select(c => new CourseDetailDto
                {
                    Id = c.Id, 
                    Title = c.Title,
                    Description = c.Description,
                    Price = c.Price,
                    ThumbnailUrl = c.ThumbnailUrl,
                    IsPublished = c.IsPublished,
                    CreatedAt = c.CreatedAt,
                    CategoryId = c.CategoryId,
                    CategoryName = c.Category.Name,
                    CategorySlug = c.Category.Slug,
                    InstructorId = c.InstructorId,
                    InstructorName = c.Instructor.FullName
                })
                .FirstOrDefaultAsync(ct);
        }
    }
}
