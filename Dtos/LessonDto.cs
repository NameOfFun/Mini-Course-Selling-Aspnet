namespace Course_Selling_System.Dtos
{
    public class LessonDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; } = null!;
        public string? VideoUrl { get; set; }
        public int OrderIndex { get; set; }
        public int DurationSeconds { get; set; }
    }
}
