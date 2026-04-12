namespace Course_Selling_System.Dtos
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAtUtc { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
