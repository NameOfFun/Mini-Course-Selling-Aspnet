using Course_Selling_System.Dtos;
using Course_Selling_System.Models;
using Course_Selling_System.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Course_Selling_System.Service.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly CourseSellingDbContext _context;
        public CategoryService(CourseSellingDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken ct = default)
        {
            return await _context.Categories.AsNoTracking().OrderBy(c => c.Name).Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug
            }).ToListAsync(ct);
        }
    }
}
