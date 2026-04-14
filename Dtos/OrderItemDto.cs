namespace Course_Selling_System.Dtos
{
    public class OrderItemDto
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = null!;
        public decimal PriceAtPurchase { get; set; }
    }
}
