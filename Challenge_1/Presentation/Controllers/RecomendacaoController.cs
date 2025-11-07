using Challenge_1.Infrastructure.ML;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Challenge_1.Presentation.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [SwaggerTag("Recomendação de Moto - ML.NET")]
    public class RecomendacaoController : ControllerBase
    {
        private readonly ModeloRecomendacao _recomendador;

        public RecomendacaoController()
        {
            _recomendador = new ModeloRecomendacao();
        }

        [HttpPost("moto")]
        [SwaggerOperation(
            Summary = "Recomenda uma moto para o usuário",
            Description = "Gera uma recomendação personalizada de moto com base no perfil do usuário e no histórico de afinidade utilizando ML.NET.")]
        [SwaggerResponse(statusCode: 200, description: "Recomendação gerada com sucesso.")]
        [SwaggerResponse(statusCode: 400, description: "Requisição inválida ou parâmetros incorretos.")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno ao gerar recomendação.")]
        public IActionResult Recomendar([FromBody] dynamic entrada)
        {
            try
            {
                if (entrada == null)
                    return BadRequest(new { Erro = "Corpo da requisição não pode ser nulo." });

                int usuarioId = (int)entrada.usuarioId;
                int motoId = (int)entrada.motoId;

                var score = _recomendador.RecomendarMoto(usuarioId, motoId);

                return Ok(new
                {
                    UsuarioId = usuarioId,
                    MotoId = motoId,
                    Score = Math.Round(score, 3),
                    Observacao = "Score de afinidade gerado via modelo ML.NET"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Erro = "Falha ao processar a recomendação.",
                    Detalhes = ex.Message
                });
            }
        }
    }
}
