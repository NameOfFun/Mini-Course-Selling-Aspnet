namespace Course_Selling_System.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public IReadOnlyList<OrderItemDto> Items { get; set; } = Array.Empty<OrderItemDto>();
    }
}
