using System.ComponentModel.DataAnnotations;

namespace Course_Selling_System.Dtos
{
    public class RegisterRequest
    {
        [Required, MaxLength(255)]
        public string FullName { get; set; } = null!;
        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = null!;
        [Required, MaxLength(255)]
        public string Password { get; set; } = null!;
    }
}
