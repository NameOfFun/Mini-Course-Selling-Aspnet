using Course_Selling_System.Dtos;
using Course_Selling_System.Models;
using Course_Selling_System.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Course_Selling_System.Service.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly CourseSellingDbContext _context;
        private readonly IConfiguration _config;
        public AuthService(CourseSellingDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<(bool Success, string? Error, AuthResponse? Data)> LoginAsync(LoginRegister login, CancellationToken ct = default)
        {
            var user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == login.Email, ct);

            if (user is null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
                return (false, "Invalid email or password", null);

            var response = CreateToken(user);
            return (true, null, response);
        }

        public async Task<(bool Success, string? Error, AuthResponse? Data)> RegisterAsync(RegisterRequest register, CancellationToken ct = default)
        {
            var exists = await _context.Users.AsNoTracking()
                .AnyAsync(u => u.Email == register.Email , ct);

            if (exists)
            {
                return (false, "Email already used", null);
            }

            var user = new User
            {
                Email = register.Email,
                FullName = register.FullName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password),
                Role = "Student",
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(ct);

            var response = CreateToken(user);
            return (true, null, response);
        }

        private AuthResponse CreateToken(User user)
        {
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SigningKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expireMinutes = int.TryParse(_config["Jwt:ExpireMinutes"], out var m) ? m : 3;
            var expires = DateTime.UtcNow.AddMinutes(expireMinutes);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expires,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponse
            {
                Token = tokenString,
                ExpiresAtUtc = expires,
                UserId = user.Id,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
