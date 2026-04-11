namespace Course_Selling_System.Dtos
{
    public class CourseDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ThumbnailUrl { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategorySlug { get; set; } = null!;
        public int InstructorId { get; set; }
        public string InstructorName { get; set; } = null!;
    }
}
