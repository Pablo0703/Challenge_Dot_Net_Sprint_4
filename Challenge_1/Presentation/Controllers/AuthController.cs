using Challenge_1.Application.Service.Auth;
using Challenge_1.Presentation.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Challenge_1.Presentation.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null)
                return BadRequest("Requisição inválida.");

            // Simulação de login (em produção, validar via banco)
            if (request.Username == "admin" && request.Password == "12345")
            {
                var token = _jwtService.GenerateToken(request.Username, "Administrador");
                return Ok(new { token });
            }

            return Unauthorized(new { message = "Usuário ou senha inválidos" });
        }
    }
}
