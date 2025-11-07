using Challenge_1.Application.Service.Auth;
using Challenge_1.Presentation.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Challenge_1.Doc.Samples;

namespace Challenge_1.Presentation.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        /// <summary>
        /// Realiza a autenticação do usuário e retorna o token JWT.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /api/v1/Auth/login
        ///     {
        ///         "username": "admin",
        ///         "password": "12345"
        ///     }
        ///
        /// Utilize o token retornado no cabeçalho das próximas requisições:
        /// 
        ///     Authorization: Bearer {token}
        /// </remarks>
        [AllowAnonymous]
        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Autenticação de usuário",
            Description = "Realiza o login de um usuário e retorna o token JWT para acesso aos endpoints protegidos."
        )]
        [SwaggerRequestExample(typeof(LoginRequest), typeof(LoginRequestSample))]
        [SwaggerResponseExample(statusCode: 200, typeof(AuthResponseSample))]
        [SwaggerResponse(statusCode: 200, description: "Usuário autenticado com sucesso", type: typeof(AuthResponseDTO))]
        [SwaggerResponse(statusCode: 400, description: "Requisição inválida")]
        [SwaggerResponse(statusCode: 401, description: "Usuário ou senha inválidos")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno no servidor")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null)
                return BadRequest(new { message = "Requisição inválida." });

            // Simulação de login (em produção, validar via banco de dados)
            if (request.Username == "admin" && request.Password == "12345")
            {
                var token = _jwtService.GenerateToken(request.Username, "Administrador");

                var response = new AuthResponseDTO
                {
                    Token = token,
                    Usuario = request.Username,
                    Perfil = "Administrador"
                };

                return Ok(response);
            }

            return Unauthorized(new { message = "Usuário ou senha inválidos." });
        }
    }
}
