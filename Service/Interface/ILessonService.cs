using Course_Selling_System.Dtos;

namespace Course_Selling_System.Service.Interface
{
    public interface ILessonService
    {
        Task<IEnumerable<LessonDto>> GetLessonsForPublishedCourseAsyunc(int courseId, CancellationToken ct = default);
    }
}
