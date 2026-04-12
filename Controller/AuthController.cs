using Course_Selling_System.Dtos;
using Course_Selling_System.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Course_Selling_System.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var (ok, err, data) = await _auth.RegisterAsync(request, ct);
            if (!ok)
            {
                return Conflict(new { message = err });
            }

            return Ok(data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRegister request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            var (ok, err, data) = await _auth.LoginAsync(request, ct);
            if (!ok)
            {
                return Unauthorized(new { message = err });
            }
            return Ok(data);
        }
    }
}
