using System.ComponentModel.DataAnnotations;

namespace Course_Selling_System.Dtos
{
    public class CreateOrderRequest
    {
        [Required, MinLength(1)]
        public IList<int> CourseIds { get; set; } = new List<int>();
    }
}
