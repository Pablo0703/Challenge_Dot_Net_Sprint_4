using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.Annotations;

namespace Challenge_1.Presentation.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    [Produces("application/json")]
    [SwaggerTag("Health Checks - Verificação da saúde da API e dependências")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthService;

        public HealthController(HealthCheckService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet("live")]
        [SwaggerOperation(
            Summary = "Verifica se a API está ativa",
            Description = "Executa uma verificação de 'liveness' para confirmar se o serviço está de pé e respondendo às requisições.")]
        [SwaggerResponse(statusCode: 200, description: "A API está ativa e funcionando")]
        [SwaggerResponse(statusCode: 503, description: "A API está indisponível ou falhando")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno ao processar a verificação")]
        public async Task<IActionResult> Live(CancellationToken ct)
        {
            var report = await _healthService.CheckHealthAsync(r => r.Tags.Contains("live"), ct);

            var result = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    error = e.Value.Exception?.Message
                })
            };

            return report.Status == HealthStatus.Healthy
                ? Ok(result)
                : StatusCode(503, result);
        }

        [HttpGet("ready")]
        [SwaggerOperation(
            Summary = "Verifica se a API e dependências estão prontas",
            Description = "Executa uma verificação de 'readiness' para confirmar se os serviços essenciais (como o banco Oracle) estão acessíveis e prontos para uso.")]
        [SwaggerResponse(statusCode: 200, description: "A API e dependências estão saudáveis")]
        [SwaggerResponse(statusCode: 503, description: "Algum serviço dependente está indisponível")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno ao processar a verificação")]
        public async Task<IActionResult> Ready(CancellationToken ct)
        {
            var report = await _healthService.CheckHealthAsync(r => r.Tags.Contains("ready"), ct);

            var result = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    error = e.Value.Exception?.Message
                })
            };

            return report.Status == HealthStatus.Healthy
                ? Ok(result)
                : StatusCode(503, result);
        }
    }
}
