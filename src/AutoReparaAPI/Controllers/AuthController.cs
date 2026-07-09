using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.Models.Requests;

namespace AutoReparaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(AuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            if (!_authService.ValidarCredenciais(model.Username, model.Password))
                return Unauthorized(new { message = "Credenciais inválidas" });

            var token = _authService.GerarToken(model.Username);
            var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");

            return Ok(new TokenResponseModel
            {
                Token = token,
                ExpiresIn = expirationMinutes * 60
            });
        }
    }
}
