using Course_Selling_System.Dtos;

namespace Course_Selling_System.Service.Interface
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseListDto>> GetPublishedAsync(CancellationToken ct = default);
        Task<CourseDetailDto?> GetPushlishedByIdAsync(int id, CancellationToken ct = default);
    }
}
