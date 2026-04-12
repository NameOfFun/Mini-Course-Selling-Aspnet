using Course_Selling_System.Dtos;

namespace Course_Selling_System.Service.Interface
{
    public interface IAuthService
    {
        Task<(bool Success, string? Error, AuthResponse? Data)> RegisterAsync(RegisterRequest register, CancellationToken ct = default);
        Task<(bool Success, string? Error, AuthResponse? Data)> LoginAsync(LoginRegister login, CancellationToken ct = default);
    }
}
