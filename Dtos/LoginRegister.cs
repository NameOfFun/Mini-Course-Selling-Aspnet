using System.ComponentModel.DataAnnotations;

namespace Course_Selling_System.Dtos
{
    public class LoginRegister
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
