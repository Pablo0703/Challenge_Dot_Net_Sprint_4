using Challenge_1.Infrastructure.ML;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Challenge_1.Presentation.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RecomendacaoController : ControllerBase
    {
        private readonly ModeloRecomendacao _recomendador;

        public RecomendacaoController()
        {
            _recomendador = new ModeloRecomendacao();
        }

        [HttpPost("moto")]
        [SwaggerOperation(Summary = "Recomenda uma moto para o usuário", Description = "Usa o modelo ML.NET de recomendação pré-treinado.")]
        public IActionResult Recomendar([FromBody] dynamic entrada)
        {
            try
            {
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
                return BadRequest(new { Erro = ex.Message });
            }
        }
    }
}
