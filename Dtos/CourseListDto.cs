namespace Course_Selling_System.Dtos
{
    public class CourseListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string? ThumbnaiUrl { get; set; }
        public string CategoryName { get; set; } = null!;
        public string InstructorName { get; set; } = null!;
        public DateTime CreateAt { get; set; }
    }
}
